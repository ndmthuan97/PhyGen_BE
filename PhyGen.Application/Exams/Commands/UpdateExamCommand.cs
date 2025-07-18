using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Exams.Commands
{
    public class UpdateExamCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ExamCategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Grade { get; set; }
        public int Year { get; set; }
        public int? TotalQuestionCount { get; set; }
        public int VersionCount { get; set; } = 1;
        public bool RandomizeQuestions { get; set; } = false;
        public string ImgUrl { get; set; } = string.Empty;
    }
}
