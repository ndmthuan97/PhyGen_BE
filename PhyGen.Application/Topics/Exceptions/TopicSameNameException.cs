using PhyGen.Domain.Exceptions;
using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Topics.Exceptions
{
    public class TopicSameNameException : AppException
    {
        public TopicSameNameException(): base(StatusCode.TopicSameName)
        {
        }
    }
}
