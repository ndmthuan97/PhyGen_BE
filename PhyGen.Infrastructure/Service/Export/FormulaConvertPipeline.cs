using PhyGen.Application.Exams.Interfaces;
using System.Text.RegularExpressions;

namespace PhyGen.Infrastructure.Service.Export
{
    public class FormulaConvertPipeline : IFormulaConvertPipeline
    {
        private readonly ILatexConvertService _latex;
        private readonly IMathmlToOmmlService _mathml;

        public FormulaConvertPipeline(ILatexConvertService latex, IMathmlToOmmlService mathml)
        {
            _latex = latex;
            _mathml = mathml;
        }

        public async Task<string> LatexToOmmlAsync(string latex, bool displayMode, CancellationToken ct = default)
        {
            var mathml = await _latex.ToMathMLAsync(latex, displayMode, ct);
            var omml = await _mathml.ToOmmlAsync(mathml, ct);
            return omml;
        }

        private static readonly Regex _latexRegex = new(
            @"(?<disp>\$\$([\s\S]+?)\$\$)|(?<disp2>\\\[((?:.|\n)+?)\\\])|(?<!\$)\$(?<inl1>[^\$]+)\$(?!\$)|(?<inl2>\\\((.+?)\\\))",
            RegexOptions.Compiled);

        private sealed record Seg(bool IsLatex, bool Display, string Text);

        private static List<Seg> SplitSegments(string? content)
        {
            var list = new List<Seg>();
            if (string.IsNullOrWhiteSpace(content)) return list;

            int last = 0;
            foreach (Match m in _latexRegex.Matches(content))
            {
                if (m.Index > last)
                    list.Add(new Seg(false, false, content.Substring(last, m.Index - last)));

                string latex =
                      m.Groups["disp"].Success ? m.Groups["disp"].Captures[0].Value
                    : m.Groups["disp2"].Success ? m.Groups["disp2"].Captures[0].Value
                    : m.Groups["inl1"].Success ? m.Groups["inl1"].Captures[0].Value
                    : m.Groups["inl2"].Success ? m.Groups["inl2"].Captures[0].Value
                    : string.Empty;

                bool display = m.Groups["disp"].Success || m.Groups["disp2"].Success;

                list.Add(new Seg(true, display, latex.Trim()));
                last = m.Index + m.Length;
            }

            if (last < content.Length)
                list.Add(new Seg(false, false, content[last..]));

            return list;
        }

        public async Task<List<FormulaSegment>> ConvertContentToSegmentsAsync(
            string? content,
            CancellationToken ct = default)
        {
            var result = new List<FormulaSegment>();
            var segs = SplitSegments(content);

            foreach (var s in segs)
            {
                if (!s.IsLatex)
                {
                    if (!string.IsNullOrEmpty(s.Text))
                        result.Add(new FormulaSegment(false, false, s.Text));
                    continue;
                }

                var omml = await LatexToOmmlAsync(s.Text, s.Display, ct);
                result.Add(new FormulaSegment(true, s.Display, omml));
            }

            return result;
        }

        public async Task<string> MergeContentWithOmmlAsync(string? content, CancellationToken ct = default)
        {
            var segs = await ConvertContentToSegmentsAsync(content, ct);
            var sb = new System.Text.StringBuilder();

            foreach (var s in segs)
            {
                if (s.IsOmml)
                {
                    sb.Append(s.Value);
                }
                else
                {
                    sb.Append(s.Value);
                }
            }

            return sb.ToString();
        }
    }
}
