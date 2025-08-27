using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Exams.Interfaces
{
    public interface IMathmlToOmmlService
    {
        Task<string> ToOmmlAsync(string mathml, CancellationToken ct = default);
    }
}
