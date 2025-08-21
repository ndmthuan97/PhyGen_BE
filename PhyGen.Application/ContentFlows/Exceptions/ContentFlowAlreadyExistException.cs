using PhyGen.Domain.Exceptions;
using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentFlows.Exceptions
{
    public class ContentFlowAlreadyExistException : AppException
    {
        public ContentFlowAlreadyExistException() : base(StatusCode.ContentFlowAlreadyExist)
        {
        }
    }
}
