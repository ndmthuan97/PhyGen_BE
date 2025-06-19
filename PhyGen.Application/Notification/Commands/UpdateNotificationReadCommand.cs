using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Notification.Commands
{
    public class UpdateNotificationReadCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
    }
}
