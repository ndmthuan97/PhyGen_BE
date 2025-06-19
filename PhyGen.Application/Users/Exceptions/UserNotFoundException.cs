using PhyGen.Domain.Exceptions;
using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Users.Exceptions
{
    public class UserNotFoundException : AppException
    {
        public UserNotFoundException() : base(StatusCode.UserNotFound) { }
    }
}
