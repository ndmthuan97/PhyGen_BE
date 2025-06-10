using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ExamCategoryChapters.Commands
{
    public class UpdateExamCategoryChapterCommand : IRequest<Unit>
    {
        public Guid ExamCategoryChapterId { get; set; }
        public Guid ExamCategoryId { get; set; }
        public Guid ChapterId { get; set; }
    }
    {
    }
}
