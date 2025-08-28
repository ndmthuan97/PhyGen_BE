using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using OMath = DocumentFormat.OpenXml.Math;
using PhyGen.Application.Exams.Interfaces;
using PhyGen.Application.Exams.Models;

namespace PhyGen.Infrastructure.Service.Export
{
    public class ExamExportService : IExamExportService
    {
        private readonly IFormulaConvertPipeline _formula;
        private readonly IHttpClientFactory _httpClientFactory;

        // Thứ tự phần theo FE
        private static readonly string[] ORDER = new[] { "MultipleChoice", "TrueFalse", "ShortAnswer", "Essay" };

        public ExamExportService(IFormulaConvertPipeline formula, IHttpClientFactory httpClientFactory)
        {
            _formula = formula;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<byte[]> ExportExamToWordAsync(ExamExportModel model, CancellationToken ct = default)
        {
            using var ms = new MemoryStream();
            using var doc = WordprocessingDocument.Create(ms, WordprocessingDocumentType.Document, true);

            var main = doc.AddMainDocumentPart();
            main.Document = new Document(new Body());
            var body = main.Document.Body!;

            // Cấu hình trang + style & header/footer (nếu bạn đang dùng)
            ConfigureDocument(main);
            CreateHeaderFooter(main);

            // ===== Header khung đề =====
            AppendHeader(body, model);

            // ===== Thông tin thí sinh =====
            body.Append(
                BuildMetaLine("Họ và tên thí sinh:\t.........................................................................................."),
                BuildMetaLine("Lớp:\t.................................\tSBD:\t.......................")
            );

            // ===== Title phần I =====
            body.Append(new Paragraph(new Run(new Text("I. TRẮC NGHIỆM KHÁCH QUAN")))
            {
                ParagraphProperties = new ParagraphProperties(new ParagraphStyleId { Val = "SectionHeading" })
            });

            // ===== Group theo SectionType và chỉ in tiêu đề phần 1 lần =====
            int questionNo = 1;
            int mcCount = 0, tfCount = 0, saCount = 0;

            var grouped = model.Sections
                .Where(s => !string.IsNullOrWhiteSpace(s.SectionType))
                .GroupBy(s => s.SectionType!, StringComparer.OrdinalIgnoreCase)
                .OrderBy(g => Array.IndexOf(ORDER, g.Key))  // ORDER = ["MultipleChoice", "TrueFalse", "ShortAnswer", "Essay"]
                .ToList();

            foreach (var g in grouped)
            {
                // In tiêu đề phần một lần cho group này
                var partTitle = g.Key switch
                {
                    "MultipleChoice" => "Phần 1. Trắc nghiệm nhiều lựa chọn",
                    "TrueFalse" => "Phần 2. Trắc nghiệm Đúng/Sai",
                    "ShortAnswer" => "Phần 3. Trắc nghiệm trả lời ngắn",
                    "Essay" => "Phần II: TỰ LUẬN",
                    _ => g.Key
                };
                body.Append(new Paragraph(new Run(new Text(partTitle)))
                {
                    ParagraphProperties = new ParagraphProperties(new ParagraphStyleId { Val = "SectionHeading" })
                });

                // Tính tổng số câu của toàn group để dùng vẽ bảng đáp án
                var n = g.Sum(s => s.Questions?.Count ?? 0);
                switch (g.Key)
                {
                    case "MultipleChoice": mcCount += n; break;
                    case "TrueFalse": tfCount += n; break;
                    case "ShortAnswer": saCount += n; break;
                }

                // Duyệt tất cả section (giữ thứ tự DisplayOrder) và in câu hỏi
                foreach (var sec in g.OrderBy(s => s.DisplayOrder))
                {
                    foreach (var q in sec.Questions ?? Enumerable.Empty<QuestionExportDto>())
                    {
                        // 1) Câu hỏi (plain text + LaTeX chuyển OMML)
                        await AppendQuestionWithLatexAsync(main, body, questionNo, q.Content ?? "", ct);

                        // 2) Ảnh của câu hỏi
                        if (q.ImageUrls is { Count: > 0 })
                        {
                            foreach (var url in q.ImageUrls)
                            {
                                var file = await TryDownloadAsync(url, ct);
                                if (file is null) continue;

                                var runImage = BuildImageRun(main, file.Value.bytes, file.Value.type);
                                var pImg = new Paragraph(runImage)
                                {
                                    ParagraphProperties = new ParagraphProperties(
                                        new Justification() { Val = JustificationValues.Center },
                                        new SpacingBetweenLines { Before = "60", After = "60" }
                                    )
                                };
                                body.Append(pImg);
                            }
                        }

                        // 3) Phương án (nếu MC/TF)
                        if (g.Key is "MultipleChoice" or "TrueFalse")
                        {
                            var answers = BuildAnswers(q);
                            foreach (var (label, text) in answers)
                            {
                                var pAns = new Paragraph
                                {
                                    ParagraphProperties = new ParagraphProperties(
                                        new SpacingBetweenLines { Before = "30", After = "30" }
                                    )
                                };
                                await AppendTextOrOmmlAsync(pAns, $"{label}. {text}", ct);
                                body.Append(pAns);
                            }
                        }

                        // khoảng trắng giữa các câu
                        body.Append(new Paragraph
                        {
                            ParagraphProperties = new ParagraphProperties(new SpacingBetweenLines { After = "60" })
                        });

                        questionNo++;
                    }
                }
            }

            // ===== BẢNG ĐÁP ÁN =====
            AppendAnswerTables(body, mcCount, tfCount, saCount);

            main.Document.Save();
            doc.Dispose();
            return ms.ToArray();
        }

        // ===================== Trang & Style =====================

        private static void ConfigureDocument(MainDocumentPart main)
        {
            // Section (A4; margin 2cm)
            var sectProps = new SectionProperties(
                new PageSize() { Width = 11906, Height = 16838 }, // A4 (twips)
                new PageMargin() { Top = 1134, Bottom = 1134, Left = 1134, Right = 1134, Header = 708, Footer = 708 }
            );
            main.Document.Body!.AppendChild(sectProps);

            // Styles part
            var stylesPart = main.StyleDefinitionsPart ?? main.AddNewPart<StyleDefinitionsPart>();
            stylesPart.Styles = new Styles();

            // Normal: Times New Roman 12pt; line 1.15
            var normal = new Style { Type = StyleValues.Paragraph, StyleId = "Normal", Default = true }
                .AppendChildRet(new StyleName { Val = "Normal" })
                .AppendChildRet(new UIPriority { Val = 1 })
                .AppendChildRet(new StyleRunProperties(
                    new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman", EastAsia = "Times New Roman" },
                    new FontSize { Val = "24" } // 12pt
                ))
                .AppendChildRet(new StyleParagraphProperties(
                    new SpacingBetweenLines { After = "0", Line = "276", LineRule = LineSpacingRuleValues.Auto }
                ));
            stylesPart.Styles.Append(normal);

            // ExamTitle
            var examTitle = new Style { Type = StyleValues.Paragraph, StyleId = "ExamTitle" }
                .AppendChildRet(new StyleName { Val = "ExamTitle" })
                .AppendChildRet(new StyleRunProperties(new Bold(), new FontSize { Val = "28" })) // 14pt
                .AppendChildRet(new StyleParagraphProperties(
                    new SpacingBetweenLines { Before = "120", After = "80" },
                    new Justification { Val = JustificationValues.Center }
                ));
            stylesPart.Styles.Append(examTitle);

            // SectionHeading
            var sectionHeading = new Style { Type = StyleValues.Paragraph, StyleId = "SectionHeading" }
                .AppendChildRet(new StyleName { Val = "SectionHeading" })
                .AppendChildRet(new StyleRunProperties(new Bold(), new FontSize { Val = "26" })) // 13pt
                .AppendChildRet(new StyleParagraphProperties(
                    new SpacingBetweenLines { Before = "160", After = "60" }
                ));
            stylesPart.Styles.Append(sectionHeading);

            // MetaLine
            var metaLine = new Style { Type = StyleValues.Paragraph, StyleId = "MetaLine" }
                .AppendChildRet(new StyleName { Val = "MetaLine" })
                .AppendChildRet(new StyleParagraphProperties(
                    new SpacingBetweenLines { After = "120" }
                ));
            stylesPart.Styles.Append(metaLine);

            // Mặc định Cambria Math cho công thức
            var docDefaults = stylesPart.Styles.Elements<DocDefaults>().FirstOrDefault();
            if (docDefaults == null)
            {
                docDefaults = new DocDefaults();
                stylesPart.Styles.PrependChild(docDefaults);
            }
            docDefaults.RunPropertiesDefault ??= new RunPropertiesDefault();
            docDefaults.RunPropertiesDefault.RunPropertiesBaseStyle ??= new RunPropertiesBaseStyle();
            docDefaults.RunPropertiesDefault.RunPropertiesBaseStyle.Append(
                new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman", ComplexScript = "Times New Roman" }
            );

            // Thiết lập m:mathPr để dùng Cambria Math
            var settingsPart = main.DocumentSettingsPart ?? main.AddNewPart<DocumentSettingsPart>();
            settingsPart.Settings ??= new Settings();
            var mathProps = settingsPart.Settings.Elements<OMath.MathProperties>().FirstOrDefault();
            if (mathProps == null)
            {
                mathProps = new OMath.MathProperties();
                settingsPart.Settings.Append(mathProps);
            }
            if (!mathProps.Elements<OMath.MathFont>().Any())
            {
                mathProps.Append(new OMath.MathFont { Val = "Cambria Math" });
            }
        }

        private static void CreateHeaderFooter(MainDocumentPart main)
        {
            // Header: SỞ GD&ĐT (trái) — TRƯỜNG THPT (phải)
            //var headerPart = main.AddNewPart<HeaderPart>();
            //string hid = main.GetIdOfPart(headerPart);
            //headerPart.Header = new Header(
            //    new Paragraph(
            //        new Run(new Text("SỞ GIÁO DỤC VÀ ĐÀO TẠO")),
            //        new TabChar(),
            //        new Run(new Text("TRƯỜNG THPT"))
            //    )
            //    {
            //        ParagraphProperties = new ParagraphProperties(
            //            new Tabs(new TabStop { Val = TabStopValues.Right, Position = 9000 }),
            //            new SpacingBetweenLines { After = "0" }
            //        )
            //    }
            //);

            // Footer: Trang X/Y
            var footerPart = main.AddNewPart<FooterPart>();
            string fid = main.GetIdOfPart(footerPart);
            footerPart.Footer = new Footer(
                new Paragraph(
                    new Run(new Text("Trang ")),
                    new Run(new FieldChar { FieldCharType = FieldCharValues.Begin }),
                    new Run(new FieldCode(" PAGE ")),
                    new Run(new FieldChar { FieldCharType = FieldCharValues.End }),
                    new Run(new Text("/")),
                    new Run(new FieldChar { FieldCharType = FieldCharValues.Begin }),
                    new Run(new FieldCode(" NUMPAGES ")),
                    new Run(new FieldChar { FieldCharType = FieldCharValues.End })
                )
                {
                    ParagraphProperties = new ParagraphProperties(
                        new Justification { Val = JustificationValues.Center },
                        new SpacingBetweenLines { Before = "0", After = "0" }
                    )
                }
            );

            // Gắn vào SectionProperties đầu tiên
            var sect = main.Document.Body!.Elements<SectionProperties>().FirstOrDefault();
            if (sect == null)
            {
                sect = new SectionProperties();
                main.Document.Body.Append(sect);
            }

            sect.RemoveAllChildren<HeaderReference>();
            sect.RemoveAllChildren<FooterReference>();
            //sect.PrependChild(new HeaderReference { Id = hid, Type = HeaderFooterValues.Default });
            sect.AppendChild(new FooterReference { Id = fid, Type = HeaderFooterValues.Default });
        }

        // ===================== Header Box & Helpers =====================

        private static void AppendHeader(Body body, ExamExportModel model)
        {
            var tbl = new Table(
                new TableProperties(
                    new TableWidth() { Type = TableWidthUnitValues.Pct, Width = "5000" },
                    new TableBorders(
                        new TopBorder() { Val = BorderValues.Single, Size = 6 },
                        new BottomBorder() { Val = BorderValues.Single, Size = 6 },
                        new LeftBorder() { Val = BorderValues.Single, Size = 6 },
                        new RightBorder() { Val = BorderValues.Single, Size = 6 },
                        new InsideHorizontalBorder() { Val = BorderValues.Single, Size = 6 },
                        new InsideVerticalBorder() { Val = BorderValues.Single, Size = 6 }
                    )
                ),
                new TableGrid(new GridColumn(), new GridColumn()),
                new TableRow(
                    new TableCell(
                        BuildParagraphCenter("SỞ GIÁO DỤC VÀ ĐÀO TẠO"),
                        BuildParagraphCenter("TRƯỜNG THPT", bold: false)
                    ),
                    new TableCell(
                        new Paragraph(new Run(new Text((model.Title ?? "ĐỀ KIỂM TRA").ToUpper())))
                        {
                            ParagraphProperties = new ParagraphProperties(new ParagraphStyleId { Val = "ExamTitle" })
                        },
                        BuildParagraphCenter($"MÔN: {model.Subject ?? "VẬT LÍ"} - KHỐI: {model.Grade}", bold: true),
                        BuildParagraphCenter($"Thời gian làm bài: {model.Duration ?? "45 phút"}", bold: true),
                        BuildParagraphCenter($"Mã đề: {model.Code ?? "01"}", bold: true)
                    )
                )
            );

            body.Append(tbl);
        }

        private static Paragraph BuildParagraphCenter(string text, bool bold = false)
        {
            var rPr = new RunProperties();
            if (bold) rPr.Append(new Bold());

            var p = new Paragraph(new Run(new Text(text)) { RunProperties = rPr })
            {
                ParagraphProperties = new ParagraphProperties(new Justification() { Val = JustificationValues.Center })
            };
            return p;
        }

        private static Paragraph BuildMetaLine(string text)
        {
            return new Paragraph(new Run(new Text(text)))
            {
                ParagraphProperties = new ParagraphProperties(
                    new ParagraphStyleId { Val = "MetaLine" },
                    new Tabs(
                        new TabStop { Val = TabStopValues.Left, Position = 2160 },  // 3cm
                        new TabStop { Val = TabStopValues.Left, Position = 6480 }   // 9cm
                    )
                )
            };
        }

        // ===================== Questions & OMML =====================

        private async Task AppendQuestionWithLatexAsync(MainDocumentPart main, Body body, int number, string content, CancellationToken ct)
        {
            var p = new Paragraph
            {
                ParagraphProperties = new ParagraphProperties(
                    new SpacingBetweenLines { Before = "60", After = "60" }
                )
            };

            // prefix "Câu x. "
            p.Append(new Run(
                new Text($"Câu {number}. ") { Space = SpaceProcessingModeValues.Preserve })
            {
                RunProperties = new RunProperties(new Bold())
            });

            // nội dung (text + OMML)
            await AppendTextOrOmmlAsync(p, content, ct);
            body.Append(p);
        }

        private async Task AppendTextOrOmmlAsync(Paragraph para, string content, CancellationToken ct)
        {
            var segs = await _formula.ConvertContentToSegmentsAsync(content, ct);
            foreach (var seg in segs)
            {
                if (!seg.IsOmml)
                {
                    para.Append(new Run(new Text(seg.Value) { Space = SpaceProcessingModeValues.Preserve }));
                }
                else
                {
                    var r = new Run();
                    r.InnerXml = seg.Value; // OMML string <m:oMath>... hoặc <m:oMathPara>...
                    para.Append(r);
                }
            }
        }

        // ===================== Images =====================

        private async Task<(byte[] bytes, PartTypeInfo type)?> TryDownloadAsync(string url, CancellationToken ct)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var bytes = await client.GetByteArrayAsync(url, ct);
                var type = GuessImagePartType(url);
                return (bytes, type);
            }
            catch
            {
                return null;
            }
        }

        private static PartTypeInfo GuessImagePartType(string url)
        {
            var ext = Path.GetExtension(url).ToLowerInvariant();
            return ext switch
            {
                ".png" => ImagePartType.Png,
                ".gif" => ImagePartType.Gif,
                ".bmp" => ImagePartType.Bmp,
                ".tiff" => ImagePartType.Tiff,
                _ => ImagePartType.Jpeg
            };
        }

        private static Run BuildImageRun(MainDocumentPart main, byte[] bytes, PartTypeInfo type)
        {
            var part = main.AddImagePart(type);
            using (var s = new MemoryStream(bytes)) part.FeedData(s);
            var rId = main.GetIdOfPart(part);

            // Giới hạn chiều ngang ~12cm (vừa lề 2cm), khoá tỉ lệ
            long cx = CmToEmu(12);
            long cy = (long)(cx * 0.7);

            var element =
                new Drawing(
                    new DW.Inline(
                        new DW.Extent() { Cx = cx, Cy = cy },
                        new DW.EffectExtent() { LeftEdge = 0L, TopEdge = 0L, RightEdge = 0L, BottomEdge = 0L },
                        new DW.DocProperties() { Id = (UInt32Value)1U, Name = "Picture" },
                        new DW.NonVisualGraphicFrameDrawingProperties(new A.GraphicFrameLocks() { NoChangeAspect = true }),
                        new A.Graphic(
                            new A.GraphicData(
                                new PIC.Picture(
                                    new PIC.NonVisualPictureProperties(
                                        new PIC.NonVisualDrawingProperties() { Id = (UInt32Value)0U, Name = "img" },
                                        new PIC.NonVisualPictureDrawingProperties()
                                    ),
                                    new PIC.BlipFill(
                                        new A.Blip() { Embed = rId },
                                        new A.Stretch(new A.FillRectangle())
                                    ),
                                    new PIC.ShapeProperties(
                                        new A.Transform2D(
                                            new A.Offset() { X = 0L, Y = 0L },
                                            new A.Extents() { Cx = cx, Cy = cy }
                                        ),
                                        new A.PresetGeometry(new A.AdjustValueList())
                                        { Preset = A.ShapeTypeValues.Rectangle }
                                    )
                                )
                            )
                            { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" }
                        )
                    )
                    { DistanceFromTop = 0U, DistanceFromBottom = 0U, DistanceFromLeft = 0U, DistanceFromRight = 0U }
                );

            return new Run(element);
        }

        private static long CmToEmu(double cm) => (long)(cm * 360000);

        // ===================== Answer Helpers =====================

        private static List<(string label, string text)> BuildAnswers(QuestionExportDto q)
        {
            var list = new List<(string label, string text)>();
            void add(string? t, string label)
            {
                if (!string.IsNullOrWhiteSpace(t))
                    list.Add((label, t.Trim()));
            }

            add(q.Answer1, "A");
            add(q.Answer2, "B");
            add(q.Answer3, "C");
            add(q.Answer4, "D");
            add(q.Answer5, "E");
            add(q.Answer6, "F");
            return list;
        }

        // ===================== BẢNG ĐÁP ÁN =====================

        // Vẽ đủ 3 phần (MultipleChoice/TrueFalse/ShortAnswer)
        private static void AppendAnswerTables(Body body, int mcCount, int tfCount, int saCount)
        {
            const int chunkSize = 10;
            const int FIRST_COL_PCT = 8; // cột "Câu/Đáp án"

            if (mcCount > 0)
            {
                body.Append(BuildParagraphCenter("ĐÁP ÁN PHẦN I: TRẮC NGHIỆM NHIỀU LỰA CHỌN", bold: true));
                AppendChunkedAnswerTable(body, mcCount, chunkSize, FIRST_COL_PCT);
            }

            if (tfCount > 0)
            {
                body.Append(new Paragraph { ParagraphProperties = new ParagraphProperties(new SpacingBetweenLines { Before = "400", After = "100" }) });
                body.Append(BuildParagraphCenter("ĐÁP ÁN PHẦN II: TRẮC NGHIỆM ĐÚNG/SAI", bold: true));
                AppendChunkedAnswerTable(body, tfCount, chunkSize, FIRST_COL_PCT);
            }

            if (saCount > 0)
            {
                body.Append(new Paragraph { ParagraphProperties = new ParagraphProperties(new SpacingBetweenLines { Before = "400", After = "100" }) });
                body.Append(BuildParagraphCenter("ĐÁP ÁN PHẦN III: TRẮC NGHIỆM TRẢ LỜI NGẮN", bold: true));
                AppendChunkedAnswerTable(body, saCount, chunkSize, FIRST_COL_PCT);
            }
        }

        private static void AppendChunkedAnswerTable(Body body, int total, int chunkSize, int firstColPct)
        {
            for (int start = 1; start <= total; start += chunkSize)
            {
                int end = Math.Min(start + chunkSize - 1, total);
                int colsInChunk = end - start + 1;
                int numColPct = (100 - firstColPct) / colsInChunk;

                var headerCells = new List<TableCell> { BuildHeaderCell("Câu", firstColPct) };
                var answerCells = new List<TableCell> { BuildHeaderCell("Đáp án", firstColPct) };

                for (int i = start; i <= end; i++)
                {
                    headerCells.Add(BuildHeaderCell(i.ToString(), numColPct));
                    answerCells.Add(BuildCell("", numColPct, center: true));
                }

                var table = new Table(
                    new TableProperties(
                        new TableWidth { Type = TableWidthUnitValues.Pct, Width = "5000" },
                        new TableBorders(
                            new TopBorder { Val = BorderValues.Single, Size = 4 },
                            new BottomBorder { Val = BorderValues.Single, Size = 4 },
                            new LeftBorder { Val = BorderValues.Single, Size = 4 },
                            new RightBorder { Val = BorderValues.Single, Size = 4 },
                            new InsideHorizontalBorder { Val = BorderValues.Single, Size = 4 },
                            new InsideVerticalBorder { Val = BorderValues.Single, Size = 4 }
                        )
                    ),
                    new TableRow(headerCells),
                    new TableRow(answerCells)
                );

                body.Append(table);
                body.Append(new Paragraph { ParagraphProperties = new ParagraphProperties(new SpacingBetweenLines { After = "160" }) });
            }
        }

        private static TableCell BuildHeaderCell(string text, int widthPct)
        {
            var tc = BuildCell(text, widthPct, center: true, bold: true);
            tc.TableCellProperties ??= new TableCellProperties();
            tc.TableCellProperties.Append(new Shading { Fill = "EDEDED", Val = ShadingPatternValues.Clear, Color = "auto" });
            return tc;
        }

        private static TableCell BuildCell(string text, int widthPct, bool center = false, bool bold = false)
        {
            var rPr = new RunProperties();
            if (bold) rPr.Append(new Bold());

            var p = new Paragraph(new Run(new Text(text)) { RunProperties = rPr })
            {
                ParagraphProperties = new ParagraphProperties(
                    new SpacingBetweenLines { Before = "60", After = "60" },
                    center ? new Justification { Val = JustificationValues.Center } : null
                )
            };

            var cell = new TableCell(p)
            {
                TableCellProperties = new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = widthPct.ToString() },
                    new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }
                )
            };
            return cell;
        }
    }

    // ===== Extension nhỏ cho code gọn hơn =====
    internal static class OpenXmlExtensions
    {
        public static T AppendChildRet<T>(this T parent, OpenXmlElement child) where T : OpenXmlElement
        {
            parent.Append(child);
            return parent;
        }
    }
}
