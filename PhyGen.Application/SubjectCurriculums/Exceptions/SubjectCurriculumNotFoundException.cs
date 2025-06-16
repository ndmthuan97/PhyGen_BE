using PhyGen.Domain.Exceptions;
using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.SubjectCurriculums.Exceptions
{
    public class SubjectCurriculumNotFoundException : AppException
    {
        public SubjectCurriculumNotFoundException() : base(StatusCode.SubjectCurriculumNotFound)
        {

        }
    }
}
