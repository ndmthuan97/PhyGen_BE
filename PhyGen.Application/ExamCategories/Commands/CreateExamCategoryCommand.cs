using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ExamCategories.Commands
{
    public class CreateExamCategoryCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;
    }
}
