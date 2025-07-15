using MediatR;
using PhyGen.Application.Matrices.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Matrices.Commands
{
    public class CreateMatrixCommand : IRequest<MatrixResponse>
    {
        public Guid SubjectId { get; set; }
        public Guid ExamCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int TotalQuestionCount { get; set; }
        public int Grade { get; set; }
        public int Year { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}
