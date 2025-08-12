using MediatR;
using PhyGen.Application.SubjectBooks.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.SubjectBooks.Queries
{
    public  class GetSubjectBookByGrade : IRequest<List<SubjectBookResponse>>
    {
        public int Grade { get; set; }
        public GetSubjectBookByGrade(int grade)
        {
            Grade = grade;
        }
    }
}
