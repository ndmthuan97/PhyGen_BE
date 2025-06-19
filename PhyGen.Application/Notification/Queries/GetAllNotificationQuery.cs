using MediatR;
using PhyGen.Application.Notification.Responses;
using PhyGen.Domain.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Notification.Queries
{
    public class GetAllNotificationQuery : BaseSpecParam, IRequest<Pagination<NotificationResponse>>
    {
        public Guid Id { get; set; }
    }
}
