using MediatR;
using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Matrices.Commands
{
    public class UpdateMatrixCommand : IRequest<Unit>
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
        public StatusQEM Status { get; set; }
        public string? MatrixCode { get; set; } = string.Empty;
    }

    public class  UpdateMatrixStatusCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public StatusQEM Status { get; set; }
        }
}
