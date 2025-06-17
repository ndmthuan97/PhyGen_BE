using PhyGen.Domain.Exceptions;
using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.SubjectBooks.Exceptions
{
    public class SubjectBookSameNameException : AppException
    {
        public SubjectBookSameNameException() : base(StatusCode.SubjectBookSameName)
        {
        }
    }
}
