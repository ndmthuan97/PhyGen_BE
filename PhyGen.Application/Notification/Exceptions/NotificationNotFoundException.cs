using PhyGen.Domain.Exceptions;
using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Notification.Exceptions
{
    public class NotificationNotFoundException : AppException
        {
            public NotificationNotFoundException() : base(StatusCode.NotifcationNotFound) { }
        }
    
}
