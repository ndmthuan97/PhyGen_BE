using MediatR;
using PhyGen.Application.Notification.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Notification.Commands
{
    public class CreateNotificationCommand : IRequest<NotificationResponse>
    {
        public Guid? Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
