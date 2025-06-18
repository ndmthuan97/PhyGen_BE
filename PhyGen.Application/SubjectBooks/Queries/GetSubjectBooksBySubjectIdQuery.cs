using MediatR;
using PhyGen.Application.SubjectBooks.Responses;
using PhyGen.Domain.Specs;
using PhyGen.Domain.Specs.SubjectBooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.SubjectBooks.Queries
{
    public record GetSubjectBooksBySubjectIdQuery(SubjectBookBySubjectIdSpecParam SubjectBookBySubjectIdSpecParam) 
        : IRequest<Pagination<SubjectBookResponse>>;
}
