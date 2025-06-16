using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentItemExamCategories.Commands
{
    public class CreateContentItemExamCategoryCommand : IRequest<int>
    {
        public Guid ContentItemId { get; set; }
        public int ExamCategoryId { get; set; }
    }
}
