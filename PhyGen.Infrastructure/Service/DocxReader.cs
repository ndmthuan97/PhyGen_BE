using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

public static class DocxReader
{
    /// <summary>
    /// Đọc toàn bộ nội dung text từ .docx: Paragraph + text trong Table.
    /// Có giữ xuống dòng cơ bản; chuẩn hoá khoảng trắng để dễ gửi đi LLM.
    /// </summary>
    public static string ReadFullText(WordprocessingDocument doc)
    {
        if (doc == null) throw new ArgumentNullException(nameof(doc));

        var body = doc.MainDocumentPart?.Document?.Body;
        if (body == null) return string.Empty;

        var sb = new StringBuilder();

        foreach (var block in body.Elements())
        {
            switch (block)
            {
                case Paragraph p:
                    AppendLineIfAny(sb, GetParagraphText(p));
                    break;

                case Table t:
                    foreach (var row in t.Elements<TableRow>())
                    {
                        foreach (var cell in row.Elements<TableCell>())
                        {
                            foreach (var el in cell.Elements())
                            {
                                if (el is Paragraph p2)
                                    AppendLineIfAny(sb, GetParagraphText(p2));
                            }
                        }
                        // tách hàng
                        sb.AppendLine();
                    }
                    break;
            }
        }

        // (tuỳ chọn) đọc thêm header/footer nếu bạn muốn:
        // sb.Append(ReadHeadersAndFooters(doc));

        return NormalizeWhitespace(sb.ToString());
    }

    private static string GetParagraphText(Paragraph p)
    {
        if (p == null) return string.Empty;

        var sb = new StringBuilder();

        // Gom text từ các Run, giữ Break (xuống dòng) cơ bản
        foreach (var run in p.Elements<Run>())
        {
            foreach (var child in run.ChildElements)
            {
                switch (child)
                {
                    case Text t:
                        sb.Append(t.Text);
                        break;
                    case Break _:
                        sb.AppendLine();
                        break;
                }
            }
        }

        // Trường hợp paragraph chủ yếu là hyperlink
        if (sb.Length == 0)
        {
            foreach (var link in p.Elements<Hyperlink>())
            {
                foreach (var t in link.Descendants<Text>())
                    sb.Append(t.Text);
            }
        }

        return sb.ToString();
    }

    private static void AppendLineIfAny(StringBuilder sb, string line)
    {
        var s = (line ?? string.Empty).TrimEnd();
        if (s.Length > 0)
            sb.AppendLine(s);
        else
            sb.AppendLine(); // vẫn giữ một dòng trống để không dính câu
    }

    private static string NormalizeWhitespace(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        var s = Regex.Replace(input, @"\r\n|\r", "\n"); // CRLF -> LF
        s = Regex.Replace(s, @"[ \t]+", " ");          // gộp khoảng trắng dư
        s = Regex.Replace(s, @"\n{3,}", "\n\n");       // tối đa 2 dòng trống liên tiếp
        return s.Trim();
    }
}
