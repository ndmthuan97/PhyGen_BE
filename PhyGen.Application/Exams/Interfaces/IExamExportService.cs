using PhyGen.Application.Exams.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Exams.Interfaces
{
    public interface IExamExportService
    {
        Task<byte[]> ExportExamToWordAsync(ExamExportModel model, CancellationToken ct = default);
    }
}
