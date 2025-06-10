using MediatR;
using PhyGen.Application.ContentItemExamCategories.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentItemExamCategories.Queries
{
    public class GetContentItemExamCategoryByContentItemIdAndExamCategoryIdQuery : IRequest<ContentItemExamCategoryResponse>
    {
        public Guid ContentItemId { get; set; }
        public int ExamCategoryId { get; set; }
        public GetContentItemExamCategoryByContentItemIdAndExamCategoryIdQuery(Guid contentItemId, int examCategoryId)
        {
            ContentItemId = contentItemId;
            ExamCategoryId = examCategoryId;
        }
    }
}
