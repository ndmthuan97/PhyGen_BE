using MediatR;
using PhyGen.Application.ExamCategoryChapters.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ExamCategoryChapters.Queries
{
    public class GetExamCategoryChapterByIdQuery : IRequest<ExamCategoryChapterResponse>
    {
        public Guid Id { get; set; }
        public GetExamCategoryChapterByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
