using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Exams.Interfaces
{
    public interface ILatexConvertService
    {
        Task<string> ToMathMLAsync(string latex, bool displayMode = false, CancellationToken ct = default);
    }
}
