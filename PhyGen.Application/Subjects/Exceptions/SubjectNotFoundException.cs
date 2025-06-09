using PhyGen.Domain.Exceptions;
using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Subjects.Exceptions
{
    public class SubjectNotFoundException : AppException
    {
        public SubjectNotFoundException() : base(StatusCode.SubjectNotFound)
        {

        }
    }
}
