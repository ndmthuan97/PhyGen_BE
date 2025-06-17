using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentItemExamCategories.Commands
{
    public class UpdateContentItemExamCategoryCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public Guid ContentItemId { get; set; }
        public Guid ExamCategoryId { get; set; }
    }
}
