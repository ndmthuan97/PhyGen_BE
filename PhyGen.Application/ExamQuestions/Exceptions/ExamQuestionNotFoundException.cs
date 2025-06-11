using PhyGen.Domain.Exceptions;
using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ExamQuestions.Exceptions
{
    internal class ExamQuestionNotFoundException : AppException
    {
        public ExamQuestionNotFoundException() : base(StatusCode.ExamQuestionNotFound) { }
    }
}
