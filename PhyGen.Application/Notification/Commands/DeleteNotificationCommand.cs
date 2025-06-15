using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Notification.Commands
{
    public class DeleteNotificationCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
