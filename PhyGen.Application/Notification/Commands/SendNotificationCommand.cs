using MediatR;
using PhyGen.Application.Notification.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Notification.Commands
{
    public class SendNotificationCommand : IRequest<List<NotificationResponse>>
    {
        public int Id { get; set; }
        public Guid? UserId { get; set; } // nullable
    }
}
