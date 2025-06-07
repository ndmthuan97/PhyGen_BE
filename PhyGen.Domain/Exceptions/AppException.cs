using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Exceptions
{
    public class AppException : Exception
    {
        public StatusCode StatusCode { get; }
        public IEnumerable<string>? Errors { get; }

        public AppException(StatusCode statusCode, IEnumerable<string>? errors = null)
            : base(ResponseMessages.GetMessage(statusCode))
        {
            StatusCode = statusCode;
            Errors = errors;
        }
    }
}
