using PhyGen.Domain.Exceptions;
using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Sections.Exceptions
{
    public class SectionAlreadyExistException : AppException
    {
        public SectionAlreadyExistException() : base(StatusCode.SectionAlreadyExist) { }
    }
}
