using MediatR;
using PhyGen.Application.Exams.Exceptions;
using PhyGen.Application.Exams.Queries;
using PhyGen.Application.Exams.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Application.Notification.Queries;
using PhyGen.Application.Notification.Responses;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Exams.Handlers
{
    public class GetAllNotificationQueryHandle : IRequestHandler<GetAllNotificationQuery, List<NotificationResponse>>
    {
        private readonly INotificationRepository _notificationRepository;
        public GetAllNotificationQueryHandle(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<List<NotificationResponse>> Handle(GetAllNotificationQuery request, CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.GetAllAsync();
            var activeNotification = notification.ToList();

            return AppMapper<CoreMappingProfile>.Mapper.Map<List<NotificationResponse>>(activeNotification);
        }
    }

}
