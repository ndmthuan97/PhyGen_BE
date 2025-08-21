using MediatR;
using PhyGen.Application.ExamCategories.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ExamCategories.Commands
{
    public class CreateExamCategoryCommand : IRequest<ExamCategoryResponse>
    {
        public string Name { get; set; } = string.Empty;
        public int OrderNo { get; set; }
    }
}
