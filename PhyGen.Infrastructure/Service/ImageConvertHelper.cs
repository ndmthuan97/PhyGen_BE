using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using ImageMagick;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using D = DocumentFormat.OpenXml.Drawing;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;

public static class ImageConvertHelper
{
    private static readonly HashSet<string> NeedsConvertToPng = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/x-wmf", "image/wmf", "application/x-msmetafile",
        "image/x-emf", "image/emf",
        "image/bmp", "image/x-bmp"
    };

    public static byte[] ToCloudinaryFriendlyPngIfNeeded(
        byte[] data, string contentType, out string outContentType, out string outExt)
    {
        // Nếu là định dạng Cloudinary hay gặp (jpeg/png/webp/gif) thì giữ nguyên
        if (!NeedsConvertToPng.Contains(contentType))
        {
            outContentType = contentType;
            outExt = contentType switch
            {
                "image/jpeg" or "image/jpg" => ".jpg",
                "image/png" => ".png",
                "image/webp" => ".webp",
                "image/gif" => ".gif",
                _ => ".jpg" // fallback an toàn
            };
            return data;
        }

        // Convert sang PNG
        using var img = new MagickImage(data);
        img.Format = MagickFormat.Png;
        // Tùy chọn tối ưu web:
        img.Quality = 90;
        var converted = img.ToByteArray();

        outContentType = "image/png";
        outExt = ".png";
        return converted;
    }
    // ====== Regex & helpers nhận diện đầu câu ======
    private static readonly Regex CauBaiRegex = new Regex(
        @"^\s*(?:Câu(?:\s*hỏi)?|Bài)\s*\d+\s*[:\.\)\-]?",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex SoThuanRegex = new Regex(
        @"^\s*\d{1,3}\s*[\.\)\-]\s+",
        RegexOptions.Compiled);

    // KHÔNG coi dòng đáp án A.)/B) là đầu câu
    private static readonly Regex ChoiceLineRegex = new Regex(
        @"^\s*[A-HĐ]\s*[\.\)]\s+\S+",
        RegexOptions.Compiled);

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

    // ====== Lấy ImagePart an toàn từ bất kỳ phần (part) nào ======
    private static ImagePart? TryGetImagePart(OpenXmlPartContainer container, string relId)
    {
        try { return container.GetPartById(relId) as ImagePart; } catch { return null; }
    }

    // Tìm "part container" nơi blip nằm (MainDocumentPart / HeaderPart / v.v.)
    private static OpenXmlPartContainer GetOwnerPart(D.Blip blip, WordprocessingDocument doc)
    {
        var root = blip.Ancestors<OpenXmlPartRootElement>().FirstOrDefault();
        return (OpenXmlPartContainer?)root?.OpenXmlPart ?? doc.MainDocumentPart;
    }

    private static OpenXmlPartContainer GetOwnerPart(DocumentFormat.OpenXml.Vml.ImageData vmlImg, WordprocessingDocument doc)
    {
        var root = vmlImg.Ancestors<OpenXmlPartRootElement>().FirstOrDefault();
        return (OpenXmlPartContainer?)root?.OpenXmlPart ?? doc.MainDocumentPart;
    }

    // ====== HÀM CHÍNH: duyệt theo document order, bắt mọi ảnh, gán vào current ======
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

        // Gắn 1 ảnh vào current/pending
        void AttachImage(byte[] bytes, string contentType)
        {
            var img = new ImageItem { Data = bytes, ContentType = contentType };
            if (current == null) pendingImages.Add(img); else current.Images.Add(img);
        }

        // Duyệt THEO THỨ TỰ XUẤT HIỆN trong Body:
        // - Nếu gặp Paragraph: xử lý text + (ảnh con nếu có)
        // - Nếu gặp Blip/VML ImageData "độc lập" ở nơi khác: vẫn bắt và gắn vào current
        var body = doc.MainDocumentPart.Document.Body;

        foreach (var node in body.Descendants())
        {
            switch (node)
            {
                case Paragraph p:
                    {
                        var text = Normalize(p.InnerText);

                        if (IsQuestionStart(p, text))
                        {
                            // Bắt đầu câu hỏi mới
                            current = new QuestionWithImages { Content = text };
                            result.Add(current);

                            // Dồn ảnh pending (ảnh xuất hiện trước câu đầu tiên)
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

                        // Ảnh là con của paragraph này (inline/anchor/vml) → gắn vào current
                        // DrawingML
                        foreach (var blip in p.Descendants<D.Blip>())
                        {
                            var relId = blip.Embed?.Value ?? blip.Link?.Value; // Link nếu ảnh liên kết ngoài
                            if (string.IsNullOrEmpty(relId)) continue;

                            var owner = GetOwnerPart(blip, doc);
                            var part = TryGetImagePart(owner, relId);
                            if (part == null) continue;

                            using var s = part.GetStream();
                            using var ms = new MemoryStream();
                            s.CopyTo(ms);
                            AttachImage(ms.ToArray(), part.ContentType);
                        }

                        // VML (định dạng cũ / textbox)
                        foreach (var vml in p.Descendants<DocumentFormat.OpenXml.Vml.ImageData>())
                        {
                            var relId = vml.RelationshipId;
                            if (string.IsNullOrEmpty(relId)) continue;

                            var owner = GetOwnerPart(vml, doc);
                            var part = TryGetImagePart(owner, relId);
                            if (part == null) continue;

                            using var s = part.GetStream();
                            using var ms = new MemoryStream();
                            s.CopyTo(ms);
                            AttachImage(ms.ToArray(), part.ContentType);
                        }

                        break;
                    }

                // Trường hợp hiếm: blip xuất hiện trong container KHÔNG phải con của Paragraph
                case D.Blip blipStandalone:
                    {
                        var relId = blipStandalone.Embed?.Value ?? blipStandalone.Link?.Value;
                        if (string.IsNullOrEmpty(relId)) break;

                        var owner = GetOwnerPart(blipStandalone, doc);
                        var part = TryGetImagePart(owner, relId);
                        if (part == null) break;

                        using var s = part.GetStream();
                        using var ms = new MemoryStream();
                        s.CopyTo(ms);
                        AttachImage(ms.ToArray(), part.ContentType);
                        break;
                    }

                case DocumentFormat.OpenXml.Vml.ImageData vmlStandalone:
                    {
                        var relId = vmlStandalone.RelationshipId;
                        if (string.IsNullOrEmpty(relId)) break;

                        var owner = GetOwnerPart(vmlStandalone, doc);
                        var part = TryGetImagePart(owner, relId);
                        if (part == null) break;

                        using var s = part.GetStream();
                        using var ms = new MemoryStream();
                        s.CopyTo(ms);
                        AttachImage(ms.ToArray(), part.ContentType);
                        break;
                    }
            }
        }

        // Ảnh còn pending mà chưa có câu → dồn vào câu hiện tại (nếu có)
        if (pendingImages.Count > 0)
        {
            EnsureCurrent();
            current!.Images.AddRange(pendingImages);
            pendingImages.Clear();
        }

        // Làm sạch
        foreach (var q in result)
            q.Content = Normalize(q.Content);

        // Bỏ câu rỗng hoàn toàn
        result = result
            .Where(q => !string.IsNullOrWhiteSpace(q.Content) || (q.Images?.Count ?? 0) > 0)
            .ToList();

        return result;
    }
    public class QuestionWithImages
    {
        public string Content { get; set; } = string.Empty;   // tránh CS8618
        public List<ImageItem> Images { get; set; } = new();  // List<ImageItem> (không phải byte[])
    }
    public class ImageItem
    {
        public byte[] Data { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = "image/jpeg";
    }

    // ------- Helpers matching theo NỘI DUNG -------
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

    // Bỏ tiền tố "Câu/Bài + số", (1.2), [VD], "1." ở đầu để so khớp nội dung thực
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
        s = Regex.Replace(s, @"[^\p{L}\p{Nd}\s\\$]", " "); // giữ chữ/số/khoảng trắng + LaTeX
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
