using MediatR;
using PhyGen.Application.Notification.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Notification.Queries
{
    public class GetAllNotificationQuery : IRequest<List<NotificationResponse>>
    {
        public Guid Id { get; set; }
    }
}
