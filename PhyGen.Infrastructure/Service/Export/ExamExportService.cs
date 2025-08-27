using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using PhyGen.Application.Exams.Interfaces;
using PhyGen.Application.Exams.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

namespace PhyGen.Infrastructure.Service.Export
{
    public class ExamExportService : IExamExportService
    {
        private readonly IFormulaConvertPipeline _formula;
        private readonly IHttpClientFactory _httpClientFactory;

        // Thứ tự phần đúng chuẩn theo FE của bạn (dùng string SectionType)
        private static readonly string[] ORDER =
            new[] { "MultipleChoice", "TrueFalse", "ShortAnswer", "Essay" };

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

            // ===== 1) Header =====
            AppendHeader(body, model);

            // ===== 2) Thông tin thí sinh =====
            body.Append(
                BuildParagraphCenterLeft("Họ và tên thí sinh: ...................................................................................."),
                BuildParagraphCenterLeft("Lớp: .............", after: "200")
            );

            // ===== 3) Title phần I =====
            body.Append(BuildTitleParagraph("I. TRẮC NGHIỆM KHÁCH QUAN", size: "28", after: "100"));

            // ===== 4) Render sections theo ORDER =====
            int questionNo = 1;

            var orderedSections = model.Sections
                .OrderBy(s => Array.IndexOf(ORDER, s.SectionType ?? string.Empty))
                .ThenBy(s => s.DisplayOrder)
                .ToList();

            int mcCount = 0, tfCount = 0, saCount = 0;

            foreach (var sec in orderedSections)
            {
                var partTitle = sec.SectionType switch
                {
                    "MultipleChoice" => "Phần 1. Trắc nghiệm nhiều lựa chọn",
                    "TrueFalse" => "Phần 2. Trắc nghiệm Đúng/Sai",
                    "ShortAnswer" => "Phần 3. Trắc nghiệm trả lời ngắn",
                    "Essay" => "Phần II: TỰ LUẬN",
                    _ => sec.Title
                };

                body.Append(BuildTitleParagraph(partTitle, size: "26", before: "200", after: "100"));

                // ---- Cộng số câu theo loại section ----
                var n = sec.Questions?.Count ?? 0;
                switch (sec.SectionType)
                {
                    case "MultipleChoice": mcCount += n; break;
                    case "TrueFalse": tfCount += n; break;
                    case "ShortAnswer": saCount += n; break;
                }

                foreach (var q in sec.Questions ?? Enumerable.Empty<QuestionExportDto>())
                {
                    // 4.1 Câu hỏi (text + LaTeX)
                    await AppendQuestionWithLatexAsync(main, body, questionNo, q.Content ?? "", ct);

                    // 4.2 Ảnh (nếu có)
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
                                    new Justification() { Val = JustificationValues.Center }
                                )
                            };
                            body.Append(pImg);
                            body.Append(new Paragraph(new Run(new Text("")))); // spacing
                        }
                    }

                    // 4.3 Các phương án (MultipleChoice/TrueFalse)
                    if (sec.SectionType is "MultipleChoice" or "TrueFalse")
                    {
                        var answers = BuildAnswers(q);
                        foreach (var (label, text) in answers)
                        {
                            var pAns = new Paragraph();
                            await AppendTextOrOmmlAsync(pAns, $"{label}. {text}", ct);
                            body.Append(pAns);
                        }
                    }

                    body.Append(new Paragraph(new Run(new Text("")))); // spacing
                    questionNo++;
                }
            }

            // ===== 5) VẼ BẢNG ĐÁP ÁN =====
            AppendAnswerTables(body, mcCount, tfCount, saCount);

            main.Document.Save();
            doc.Dispose();
            return ms.ToArray();
        }

        #region Header helpers

        private static void AppendHeader(Body body, ExamExportModel model)
        {
            // Bảng 2 cột, viền hộp theo FE
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
                        BuildParagraphCenter("TRƯỜNG THPT"),
                        BuildParagraphCenter("🙢★🙠")
                    ),
                    new TableCell(
                        BuildParagraphCenter(model.Title?.ToUpper() ?? "ĐỀ THI", bold: true, size: "28"),
                        BuildParagraphCenter($"MÔN: {model.Subject ?? "VẬT LÍ"} - KHỐI: {model.Grade}", bold: true),
                        BuildParagraphCenter($"Thời gian làm bài: {model.Duration ?? "45 phút"}", bold: true),
                        BuildParagraphCenter($"Mã đề: {model.Code ?? "01"}", bold: true)
                    )
                )
            );

            body.Append(tbl);
            body.Append(new Paragraph(new Run(new Text("")))
            {
                ParagraphProperties = new ParagraphProperties(new SpacingBetweenLines() { After = "200" })
            });
        }

        private static Paragraph BuildParagraphCenter(string text, bool bold = false, string? size = null)
        {
            var rPr = new RunProperties();
            if (bold) rPr.Append(new Bold());
            if (!string.IsNullOrEmpty(size)) rPr.Append(new FontSize() { Val = size });

            var p = new Paragraph(new Run(new Text(text)) { RunProperties = rPr })
            {
                ParagraphProperties = new ParagraphProperties(new Justification() { Val = JustificationValues.Center })
            };
            return p;
        }

        private static Paragraph BuildParagraphCenterLeft(string text, string? after = null)
        {
            var p = new Paragraph(new Run(new Text(text)));
            if (!string.IsNullOrEmpty(after))
            {
                p.ParagraphProperties = new ParagraphProperties(new SpacingBetweenLines { After = after });
            }
            return p;
        }

        private static Paragraph BuildTitleParagraph(string text, string size, string? before = null, string? after = null)
        {
            var p = new Paragraph(
                new Run(new Text(text))
                {
                    RunProperties = new RunProperties(new Bold(), new FontSize() { Val = size })
                });

            var pp = new ParagraphProperties();
            if (!string.IsNullOrEmpty(before)) pp.Append(new SpacingBetweenLines() { Before = before });
            if (!string.IsNullOrEmpty(after)) pp.Append(new SpacingBetweenLines() { After = after });
            p.ParagraphProperties = pp;

            return p;
        }

        #endregion

        #region Questions & OMML

        private async Task AppendQuestionWithLatexAsync(MainDocumentPart main, Body body, int number, string content, CancellationToken ct)
        {
            var p = new Paragraph();
            // prefix "Câu x. "
            p.Append(new Run(
                new Text($"Câu {number}. ") { Space = SpaceProcessingModeValues.Preserve })
            {
                RunProperties = new RunProperties(new Bold())
            });

            // nội dung (text + LaTeX đã convert OMML)
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
                    r.InnerXml = seg.Value; // OMML string <m:oMath>...</m:oMath> hoặc <m:oMathPara>...</m:oMathPara>
                    para.Append(r);
                }
            }
        }

        #endregion

        #region Images

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

            // width ~ 10cm, height auto
            long cx = CmToEmu(10);
            long cy = CmToEmu(7); // tạm 7cm, hoặc bạn có thể 0 để Word auto-fit (tuỳ)

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
        #endregion

        #region Answers helper

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

        #endregion

        #region Answer tables (BẢNG ĐÁP ÁN)

        // Gọi một lần để vẽ đủ 3 phần (MultipleChoice/TrueFalse/ShortAnswer)
        private static void AppendAnswerTables(Body body, int mcCount, int tfCount, int saCount)
        {
            const int chunkSize = 10;
            const int FIRST_COL_PCT = 8; // cột "Câu/Đáp án"

            // I. Trắc nghiệm nhiều lựa chọn
            if (mcCount > 0)
            {
                body.Append(BuildParagraphCenter("ĐÁP ÁN PHẦN I: TRẮC NGHIỆM NHIỀU LỰA CHỌN", bold: true));
                AppendChunkedAnswerTable(body, mcCount, chunkSize, FIRST_COL_PCT);
            }

            // II. Đúng/Sai
            if (tfCount > 0)
            {
                body.Append(new Paragraph(new Run(new Text("")))
                {
                    ParagraphProperties = new ParagraphProperties(new SpacingBetweenLines { Before = "400", After = "100" })
                });
                body.Append(BuildParagraphCenter("ĐÁP ÁN PHẦN II: TRẮC NGHIỆM ĐÚNG/SAI", bold: true));
                AppendChunkedAnswerTable(body, tfCount, chunkSize, FIRST_COL_PCT);
            }

            // III. Trả lời ngắn
            if (saCount > 0)
            {
                body.Append(new Paragraph(new Run(new Text("")))
                {
                    ParagraphProperties = new ParagraphProperties(new SpacingBetweenLines { Before = "400", After = "100" })
                });
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

                var headerCells = new List<TableCell>();
                var answerCells = new List<TableCell>();

                // cột "Câu"
                headerCells.Add(BuildCell("Câu", firstColPct, center: true, bold: true));
                // cột "Đáp án"
                answerCells.Add(BuildCell("Đáp án", firstColPct, center: true, bold: true));

                for (int i = start; i <= end; i++)
                {
                    headerCells.Add(BuildCell(i.ToString(), numColPct, center: true));
                    answerCells.Add(BuildCell("", numColPct, center: true)); // ô trống cho đáp án
                }

                var table = new Table(
                    new TableProperties(
                        new TableWidth { Type = TableWidthUnitValues.Pct, Width = "5000" },
                        new TableBorders(
                            new TopBorder { Val = BorderValues.Single, Size = 6 },
                            new BottomBorder { Val = BorderValues.Single, Size = 6 },
                            new LeftBorder { Val = BorderValues.Single, Size = 6 },
                            new RightBorder { Val = BorderValues.Single, Size = 6 },
                            new InsideHorizontalBorder { Val = BorderValues.Single, Size = 6 },
                            new InsideVerticalBorder { Val = BorderValues.Single, Size = 6 }
                        )
                    ),
                    new TableRow(headerCells),
                    new TableRow(answerCells)
                );

                body.Append(table);
                body.Append(new Paragraph(new Run(new Text("")))
                {
                    ParagraphProperties = new ParagraphProperties(new SpacingBetweenLines { After = "200" })
                });
            }
        }

        private static TableCell BuildCell(string text, int widthPct, bool center = false, bool bold = false)
        {
            var rPr = new RunProperties();
            if (bold) rPr.Append(new Bold());

            var p = new Paragraph(new Run(new Text(text)) { RunProperties = rPr });
            if (center) p.ParagraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center });

            var cell = new TableCell(p)
            {
                TableCellProperties = new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = widthPct.ToString() }
                )
            };
            return cell;
        }

        #endregion

    }
}
