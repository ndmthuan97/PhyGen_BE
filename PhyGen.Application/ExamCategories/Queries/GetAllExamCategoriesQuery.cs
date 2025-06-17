using MediatR;
using PhyGen.Application.ExamCategories.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ExamCategories.Queries
{
    public class GetAllExamCategoriesQuery : IRequest<List<ExamCategoryResponse>>
    {
    }
}
