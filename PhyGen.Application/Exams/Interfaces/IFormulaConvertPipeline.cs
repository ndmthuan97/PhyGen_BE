using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Exams.Interfaces
{
    public sealed record FormulaSegment(bool IsOmml, bool Display, string Value);

    public interface IFormulaConvertPipeline
    {
        Task<string> LatexToOmmlAsync(string latex, bool displayMode, CancellationToken ct = default);
        Task<List<FormulaSegment>> ConvertContentToSegmentsAsync(string? content, CancellationToken ct = default);
    }
}
