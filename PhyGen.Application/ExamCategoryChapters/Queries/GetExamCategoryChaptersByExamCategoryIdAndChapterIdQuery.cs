using MediatR;
using PhyGen.Application.ExamCategoryChapters.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ExamCategoryChapters.Queries
{
    public class GetExamCategoryChaptersByExamCategoryIdAndChapterIdQuery : IRequest<ExamCategoryChapterResponse>
    {
        public Guid ExamCategoryId { get; set; }
        public Guid ChapterId { get; set; }
        public GetExamCategoryChaptersByExamCategoryIdAndChapterIdQuery(Guid examCategoryId, Guid chapterId)
        {
            ExamCategoryId = examCategoryId;
            ChapterId = chapterId;
        }
    }
}
