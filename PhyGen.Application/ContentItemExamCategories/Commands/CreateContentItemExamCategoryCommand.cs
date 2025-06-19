using MediatR;
using PhyGen.Application.ContentItemExamCategories.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentItemExamCategories.Commands
{
    public class CreateContentItemExamCategoryCommand : IRequest<ContentItemExamCategoryResponse>
    {
        public Guid ContentItemId { get; set; }
        public Guid ExamCategoryId { get; set; }
    }
}
