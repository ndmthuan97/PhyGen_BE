using MediatR;
using PhyGen.Application.ContentItemExamCategories.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentItemExamCategories.Queries
{
    public class GetContentItemExamCategoryByIdQuery : IRequest<ContentItemExamCategoryResponse>
    {
        public Guid Id { get; set; }
        public GetContentItemExamCategoryByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
