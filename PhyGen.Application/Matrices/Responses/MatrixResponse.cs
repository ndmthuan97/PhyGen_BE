using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Matrices.Responses
{
    public class MatrixResponse
    {
        public Guid Id { get; set; }
        public Guid SubjectId { get; set; }
        public Guid ExamCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int TotalQuestionCount { get; set; }
        public int Grade { get; set; }
        public int Year { get; set; }
        public string ImgUrl { get; set; } = string.Empty;
        public string? CreatedBy { get; set; }

        public StatusQEM Status { get; set; } = StatusQEM.Draft;
        public string? MatrixCode { get; set; } = string.Empty;
    }
}
