using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ImageMagick;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using D = DocumentFormat.OpenXml.Drawing;
using W = DocumentFormat.OpenXml.Wordprocessing;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;

public static class ImageConvertHelper
{
    private static class OmmlToLatex
    {
        public static string ConvertInline(OpenXmlElement oMath) => $"${ConvertElement(oMath)}$";
        public static string ConvertBlock(OpenXmlElement oMathPara) => $"$$\n{ConvertElement(oMathPara)}\n$$";

        public static string ConvertElement(OpenXmlElement el)
        {
            switch (el.LocalName)
            {
                case "oMathPara":
                case "oMath":
                    return Concat(el.Elements().Select(ConvertElement));

                case "f": // Fraction
                    {
                        var num = First(el, "num");
                        var den = First(el, "den");
                        return $"\\frac{{{ConvertChildren(num)}}}{{{ConvertChildren(den)}}}";
                    }
                case "rad": // Radical
                    {
                        var deg = ConvertChildren(First(el, "deg"));
                        var e = ConvertChildren(First(el, "e"));
                        var degStr = string.IsNullOrEmpty(deg) ? "" : $"[{deg}]";
                        return $"\\sqrt{degStr}{{{e}}}";
                    }
                case "sSup":
                    return $"{{{ConvertChildren(First(el, "e"))}}}^{{{ConvertChildren(First(el, "sup"))}}}";
                case "sSub":
                    return $"{{{ConvertChildren(First(el, "e"))}}}_{{{ConvertChildren(First(el, "sub"))}}}";
                case "sSubSup":
                    return $"{{{ConvertChildren(First(el, "e"))}}}_{{{ConvertChildren(First(el, "sub"))}}}^{{{ConvertChildren(First(el, "sup"))}}}";
                case "d": // Delimiter
                    {
                        var dPr = First(el, "dPr");
                        var begin = Attr(First(dPr, "begChr"), "val");
                        var end = Attr(First(dPr, "endChr"), "val");
                        if (string.IsNullOrEmpty(begin)) begin = "(";
                        if (string.IsNullOrEmpty(end)) end = ")";
                        var body = Concat(el.Elements().Where(c => c.LocalName != "dPr").Select(ConvertElement));
                        // bỏ khoảng trắng thừa giữa \left,\right và nội dung
                        return $"\\left{MapBracket(begin)}{body}\\right{MapBracket(end)}";
                    }
                case "nary": // Summation, Integral, etc.
                    {
                        var nPr = First(el, "naryPr");
                        var chr = Attr(First(nPr, "chr"), "val");
                        var op = MapNary(chr);
                        var sub = ConvertChildren(First(el, "sub"));
                        var sup = ConvertChildren(First(el, "sup"));
                        var e = ConvertChildren(First(el, "e"));
                        var lim = $"{(string.IsNullOrEmpty(sub) ? "" : $"_{{{sub}}}")}{(string.IsNullOrEmpty(sup) ? "" : $"^{{{sup}}}")}";
                        return $"{op}{lim} {e}";
                    }
                case "func": // Function
                    {
                        var name = ConvertChildren(First(el, "fName")).Replace(" ", "");
                        var arg = ConvertChildren(First(el, "e"));
                        if (string.IsNullOrWhiteSpace(name)) return arg;
                        return $"\\mathrm{{{name}}}\\left({arg}\\right)";
                    }
                case "m": // Matrix
                    {
                        var rowStrs = new List<string>();
                        foreach (var row in el.Elements().Where(x => x.LocalName == "mr"))
                        {
                            var cells = new List<string>();
                            foreach (var cell in row.Elements().Where(x => x.LocalName == "mc"))
                                cells.Add(ConvertChildren(cell));
                            rowStrs.Add(string.Join(" & ", cells));
                        }
                        var body = string.Join(" \\\\ ", rowStrs);
                        return $"\\begin{{matrix}} {body} \\end{{matrix}}";
                    }
                case "r": // Run
                    return Concat(el.Elements().Select(ConvertElement));
                case "t": // Text
                    return EscapeLatex(el.InnerText);
                default:
                    if (el.HasChildren) return Concat(el.ChildElements.Select(ConvertElement));
                    return EscapeLatex(el.InnerText);
            }
        }

        private static string ConvertChildren(OpenXmlElement? e) =>
            e == null ? "" : Concat(e.ChildElements.Select(ConvertElement));

        private static OpenXmlElement? First(OpenXmlElement? parent, string localName) =>
            parent?.Elements().FirstOrDefault(x => x.LocalName == localName);

        private static string Attr(OpenXmlElement? e, string localName)
        {
            if (e == null) return "";
            foreach (var attr in e.GetAttributes())
                if (attr.LocalName == localName)
                    return attr.Value ?? "";
            return "";
        }

        private static string Concat(IEnumerable<string> parts) =>
            string.Join("", parts.Where(s => !string.IsNullOrEmpty(s)));

        private static string MapBracket(string b) => b switch
        {
            "(" or ")" or "[" or "]" or "{" or "}" => b,
            "⟨" => "\\langle",
            "⟩" => "\\rangle",
            "|" => "|",
            _ => b
        };

        private static string MapNary(string? sym) => sym switch
        {
            "∑" => "\\sum",
            "∏" => "\\prod",
            "∐" => "\\coprod",
            "∫" => "\\int",
            "∮" => "\\oint",
            "⋃" => "\\bigcup",
            "⋂" => "\\bigcap",
            "⋁" => "\\bigvee",
            "⋀" => "\\bigwedge",
            _ => "\\sum"
        };

        private static string EscapeLatex(string? s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s.Replace(@"\", @"\textbackslash ")
                    .Replace("{", @"\{").Replace("}", @"\}")
                    .Replace("#", @"\#").Replace("$", @"\$")
                    .Replace("%", @"\%").Replace("&", @"\&")
                    .Replace("_", @"\_").Replace("^", @"\^{}")
                    .Replace("~", @"\~{}");
        }
    }

    // ===========================
    // ẢNH - Convert sang PNG nếu cần
    // ===========================
    private static readonly HashSet<string> NeedsConvertToPng = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/x-wmf", "image/wmf", "application/x-msmetafile",
        "image/x-emf", "image/emf",
        "image/bmp", "image/x-bmp"
    };

    public static byte[] ToCloudinaryFriendlyPngIfNeeded(
        byte[] data, string contentType, out string outContentType, out string outExt)
    {
        if (!NeedsConvertToPng.Contains(contentType))
        {
            outContentType = contentType;
            outExt = contentType switch
            {
                "image/jpeg" or "image/jpg" => ".jpg",
                "image/png" => ".png",
                "image/webp" => ".webp",
                "image/gif" => ".gif",
                _ => ".jpg"
            };
            return data;
        }

        using var img = new MagickImage(data);
        img.Strip();
        img.Format = MagickFormat.Png8;
        img.Quality = 90;

        const int maxW = 2500, maxH = 2500;
        if (img.Width > maxW || img.Height > maxH)
            img.Resize(new MagickGeometry(maxW, maxH) { IgnoreAspectRatio = false });

        var converted = img.ToByteArray();
        outContentType = "image/png";
        outExt = ".png";
        return converted;
    }

    // ===========================
    // LỌC ẢNH BLANK CANVAS (MỚI)
    // ===========================
    private static bool IsBlankCanvas(byte[] bytes)
    {
        try
        {
            using var img = new MagickImage(bytes);

            // Histogram cơ bản để ước lượng tỉ lệ trắng/đen/khác
            var hist = img.Histogram();
            if (hist == null || hist.Count == 0) return true;

            long total = 0, whitesTrans = 0, blacks = 0, others = 0;
            foreach (var kv in hist)
            {
                var c = kv.Key;
                long cnt = (long)kv.Value;
                total += cnt;

                double a01 = c.A / (double)Quantum.Max; // 0..1
                bool isTrans = a01 > 0.90;

                double r = c.R / (double)Quantum.Max * 255.0;
                double g = c.G / (double)Quantum.Max * 255.0;
                double b = c.B / (double)Quantum.Max * 255.0;

                bool isWhiteish = (r >= 235 && g >= 235 && b >= 235); // cho phép trắng-xám
                bool isBlackish = (r <= 10 && g <= 10 && b <= 10);

                if (isTrans || isWhiteish) whitesTrans += cnt;
                else if (isBlackish) blacks += cnt;
                else others += cnt;
            }

            if (total == 0) return true;

            double pWhiteTrans = whitesTrans / (double)total;
            double pBlack = blacks / (double)total;
            double pOther = others / (double)total;

            // Thống kê biến thiên màu (API mới)
            var stats = img.Statistics();
            IChannelStatistics? rStats = stats?.GetChannel(PixelChannel.Red);
            IChannelStatistics? gStats = stats?.GetChannel(PixelChannel.Green);
            IChannelStatistics? bStats = stats?.GetChannel(PixelChannel.Blue);

            double norm = 1.0 / Quantum.Max;
            double stdR = (rStats?.StandardDeviation ?? 0) * norm;
            double stdG = (gStats?.StandardDeviation ?? 0) * norm;
            double stdB = (bStats?.StandardDeviation ?? 0) * norm;
            double stdAvg = (stdR + stdG + stdB) / 3.0;

            // Số màu
            var colors = img.TotalColors;

            // Quy tắc coi là "blank"
            bool whiteDominates = pWhiteTrans >= 0.998 && pBlack <= 0.001 && pOther <= 0.001;
            bool veryLowVarOrFewColors = (stdAvg <= 0.008) || (colors <= 6);

            return whiteDominates && veryLowVarOrFewColors;
        }
        catch
        {
            // Không đọc được -> không loại nhầm
            return false;
        }
    }

    // ===========================
    // Regex nhận diện câu hỏi/đáp án
    // ===========================
    private static readonly Regex CauBaiRegex = new(@"^\s*(?:Câu(?:\s*hỏi)?|Bài)\s*\d+\s*[:\.\)\-]?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex SoThuanRegex = new(@"^\s*\d{1,3}\s*[\.\)\-]\s+", RegexOptions.Compiled);
    private static readonly Regex ChoiceLineRegex = new(@"^\s*[A-HĐ]\s*[\.\)]\s+\S+", RegexOptions.Compiled);

    private static string Normalize(string? s)
        => string.IsNullOrWhiteSpace(s) ? "" : Regex.Replace(s, @"\s+", " ").Trim();

    private static bool LooksLongEnoughForQuestion(string text)
    {
        if (text.Length >= 20) return true;
        if (text.Contains('?')) return true;
        if (Regex.IsMatch(text, @"[:;]{1,}")) return true;
        if (Regex.IsMatch(text, @"\b(Cho|Tính|Xác định|Giải|Hãy|Một)\b", RegexOptions.IgnoreCase)) return true;
        return false;
    }

    private static bool IsTopLevelNumbered(Paragraph p)
    {
        var np = p.ParagraphProperties?.NumberingProperties;
        if (np?.NumberingLevelReference?.Val == null) return false;
        return (int)np.NumberingLevelReference.Val.Value == 0;
    }

    private static bool IsQuestionStart(Paragraph p, string normalizedText)
    {
        if (string.IsNullOrEmpty(normalizedText)) return false;
        if (ChoiceLineRegex.IsMatch(normalizedText)) return false;
        if (p.ParagraphProperties?.NumberingProperties != null && IsTopLevelNumbered(p)) return true;
        if (CauBaiRegex.IsMatch(normalizedText)) return true;
        if (SoThuanRegex.IsMatch(normalizedText) && LooksLongEnoughForQuestion(normalizedText)) return true;
        return false;
    }

    private static bool IsChoiceParagraph(Paragraph p)
    {
        var tx = Normalize(p.InnerText);
        if (ChoiceLineRegex.IsMatch(tx)) return true;
        if (string.IsNullOrWhiteSpace(tx)) return false;
        if (tx.Length <= 3) return true;
        return false;
    }

    private static string GetParagraphTextWithMath(Paragraph p)
    {
        var sb = new StringBuilder();
        foreach (var child in p.ChildElements)
        {
            switch (child)
            {
                case OpenXmlElement e when e.NamespaceUri == "http://schemas.openxmlformats.org/officeDocument/2006/math":
                    sb.Append(e.LocalName == "oMath"
                        ? OmmlToLatex.ConvertInline(e)
                        : OmmlToLatex.ConvertBlock(e));
                    break;
                case W.Run wr:
                    foreach (var rChild in wr.ChildElements)
                    {
                        if (rChild.NamespaceUri == "http://schemas.openxmlformats.org/officeDocument/2006/math")
                        {
                            sb.Append(rChild.LocalName == "oMath"
                                ? OmmlToLatex.ConvertInline(rChild)
                                : OmmlToLatex.ConvertBlock(rChild));
                        }
                        else if (rChild is W.Text wt)
                        {
                            sb.Append(wt.Text);
                        }
                        else
                        {
                            sb.Append(rChild.InnerText);
                        }
                    }
                    break;
                default:
                    sb.Append(child.InnerText);
                    break;
            }
        }
        return Normalize(sb.ToString());
    }

    public static List<QuestionWithImages> ExtractQuestionsWithImages(WordprocessingDocument doc)
    {
        var result = new List<QuestionWithImages>();
        QuestionWithImages? current = null;
        var pendingImages = new List<ImageItem>();

        void EnsureCurrent()
        {
            if (current == null)
            {
                current = new QuestionWithImages { Content = "" };
                result.Add(current);
            }
        }

        void AttachImage(byte[] bytes, string contentType)
        {
            try
            {
                using var probe = new MagickImage(bytes);
                if (probe.Width <= 40 && probe.Height <= 40) return; // icon rất nhỏ
                var hist = probe.Histogram();
                if (hist != null && hist.Count <= 8 && (probe.Width * probe.Height) <= 20000)
                    return; // ảnh rất nhỏ & rất ít màu -> bỏ
            }
            catch { }

            var img = new ImageItem { Data = bytes, ContentType = contentType };
            if (current == null) pendingImages.Add(img); else current.Images.Add(img);
        }

        byte[]? ReadImageBytes(OpenXmlPartContainer owner, string relId, out string? ct)
        {
            ct = null;
            if (string.IsNullOrWhiteSpace(relId)) return null;
            var part = TryGetImagePart(owner, relId);
            if (part == null) return null;
            ct = part.ContentType;
            using var s = part.GetStream();
            using var ms = new MemoryStream();
            s.CopyTo(ms);
            return ms.ToArray();
        }

        var body = doc.MainDocumentPart!.Document.Body!;
        foreach (var p in body.Elements<Paragraph>())
        {
            var text = GetParagraphTextWithMath(p);
            if (IsQuestionStart(p, text))
            {
                current = new QuestionWithImages { Content = text };
                result.Add(current);

                if (pendingImages.Count > 0)
                {
                    current.Images.AddRange(pendingImages);
                    pendingImages.Clear();
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(text))
                {
                    EnsureCurrent();
                    if (current!.Content.Length > 0) current.Content += "\n";
                    current.Content += text;
                }
            }

            bool skipImages = IsChoiceParagraph(p);

            if (!skipImages)
            {
                foreach (var blip in p.Descendants<D.Blip>())
                {
                    var relId = blip.Embed?.Value ?? blip.Link?.Value;
                    var owner = GetOwnerPart(blip, doc);
                    var bytes = ReadImageBytes(owner, relId!, out var ct);
                    if (bytes != null && ct != null)
                    {
                        // CHẶN ẢNH BLANK-CANVAS TRƯỚC
                        if (IsBlankCanvas(bytes)) continue;
                        AttachImage(bytes, ct);
                    }
                }

                foreach (var vml in p.Descendants<DocumentFormat.OpenXml.Vml.ImageData>())
                {
                    var relId = vml.RelationshipId;
                    var owner = GetOwnerPart(vml, doc);
                    var bytes = ReadImageBytes(owner, relId!, out var ct);
                    if (bytes != null && ct != null)
                    {
                        if (IsBlankCanvas(bytes)) continue;
                        AttachImage(bytes, ct);
                    }
                }
            }
        }

        foreach (var blipStandalone in body.Descendants<D.Blip>()
                 .Where(b => b.Ancestors<Paragraph>().FirstOrDefault() is null))
        {
            var relId = blipStandalone.Embed?.Value ?? blipStandalone.Link?.Value;
            var owner = GetOwnerPart(blipStandalone, doc);
            var bytes = ReadImageBytes(owner, relId!, out var ct);
            if (bytes != null && ct != null)
            {
                if (IsBlankCanvas(bytes)) continue;
                AttachImage(bytes, ct);
            }
        }

        foreach (var vmlStandalone in body
                 .Descendants<DocumentFormat.OpenXml.Vml.ImageData>()
                 .Where(v => v.Ancestors<Paragraph>().FirstOrDefault() is null))
        {
            var relId = vmlStandalone.RelationshipId;
            var owner = GetOwnerPart(vmlStandalone, doc);
            var bytes = ReadImageBytes(owner, relId!, out var ct);
            if (bytes != null && ct != null)
            {
                if (IsBlankCanvas(bytes)) continue;
                AttachImage(bytes, ct);
            }
        }

        if (pendingImages.Count > 0)
        {
            EnsureCurrent();
            current!.Images.AddRange(pendingImages);
            pendingImages.Clear();
        }

        foreach (var q in result)
            q.Content = Normalize(q.Content);

        result = result
            .Where(q => !string.IsNullOrWhiteSpace(q.Content) || (q.Images?.Count ?? 0) > 0)
            .ToList();

        return result;
    }

    private static ImagePart? TryGetImagePart(OpenXmlPartContainer container, string relId)
    {
        try { return container.GetPartById(relId) as ImagePart; } catch { return null; }
    }

    private static OpenXmlPartContainer GetOwnerPart(D.Blip blip, WordprocessingDocument doc)
    {
        var root = blip.Ancestors<OpenXmlPartRootElement>().FirstOrDefault();
        return (OpenXmlPartContainer?)root?.OpenXmlPart ?? doc.MainDocumentPart!;
    }

    private static OpenXmlPartContainer GetOwnerPart(DocumentFormat.OpenXml.Vml.ImageData vmlImg, WordprocessingDocument doc)
    {
        var root = vmlImg.Ancestors<OpenXmlPartRootElement>().FirstOrDefault();
        return (OpenXmlPartContainer?)root?.OpenXmlPart ?? doc.MainDocumentPart!;
    }

    // ===========================
    // Data models & helpers
    // ===========================
    public class QuestionWithImages
    {
        public string Content { get; set; } = string.Empty;
        public List<ImageItem> Images { get; set; } = new();
    }

    public class ImageItem
    {
        public byte[] Data { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = "image/jpeg";
    }

    static string RemoveDiacritics(string input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;
        var normalized = input.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(normalized.Length);
        foreach (var ch in normalized)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
            if (uc != UnicodeCategory.NonSpacingMark) sb.Append(ch);
        }
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }

    static string StripLeadingLabels(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return string.Empty;
        s = s.Trim();
        s = Regex.Replace(s, @"^\s*(?:Câu(?:\s*hỏi)?|Bài)\s*\d{1,3}\s*(?:[\.\):\-]|(?:\[[^\]]+\]))?\s*", "", RegexOptions.IgnoreCase);
        s = Regex.Replace(s, @"^\s*\(\s*\d+(?:[\,\.]\d+)?\s*(?:đ|điểm)?\s*\)\s*", "", RegexOptions.IgnoreCase);
        s = Regex.Replace(s, @"^\s*\[\s*[^\]]+\s*\]\s*", "", RegexOptions.IgnoreCase);
        s = Regex.Replace(s, @"^\s*\d{1,3}\s*[\.\)\-]\s*", "");
        return s.Trim();
    }

    public static string Canonicalize(string? s)
    {
        if (string.IsNullOrWhiteSpace(s)) return string.Empty;
        s = s.Replace("\r", " ").Replace("\n", " ");
        s = Regex.Replace(s, @"\s+", " ").Trim();
        s = StripLeadingLabels(s);
        s = RemoveDiacritics(s).ToLowerInvariant();
        s = Regex.Replace(s, @"[^\p{L}\p{Nd}\s\\$]", " ");
        s = Regex.Replace(s, @"\s+", " ").Trim();
        return s;
    }

    public static string ExtractHead(string s, int maxChars = 64)
    {
        s = Canonicalize(s);
        return s.Length <= maxChars ? s : s.Substring(0, maxChars);
    }

    static double ContainsScore(string a, string b)
    {
        if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b)) return 0.0;
        if (a.Contains(b)) return Math.Min(1.0, (double)b.Length / Math.Max(16, a.Length));
        if (b.Contains(a)) return Math.Min(1.0, (double)a.Length / Math.Max(16, b.Length));
        return 0.0;
    }

    static double JaccardTokens(string a, string b)
    {
        var ta = new HashSet<string>(Regex.Split(a, @"\s+").Where(t => t.Length >= 2));
        var tb = new HashSet<string>(Regex.Split(b, @"\s+").Where(t => t.Length >= 2));
        if (ta.Count == 0 || tb.Count == 0) return 0.0;
        var inter = ta.Intersect(tb).Count();
        var union = ta.Union(tb).Count();
        return union == 0 ? 0.0 : (double)inter / union;
    }

    public static double MatchScore(string src, string tgt)
    {
        var c = ContainsScore(src, tgt);
        var j = JaccardTokens(src, tgt);
        return 0.65 * c + 0.35 * j;
    }

    public static string Sha256Hex(byte[] data)
    {
        using var sha = SHA256.Create();
        var hash = sha.ComputeHash(data);
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }
}
