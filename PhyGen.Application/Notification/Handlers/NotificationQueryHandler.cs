using AutoMapper;
using MediatR;
using Org.BouncyCastle.Asn1.Ocsp;
using PhyGen.Application.Exams.Exceptions;
using PhyGen.Application.Exams.Queries;
using PhyGen.Application.Exams.Responses;
using PhyGen.Application.Mapping;
using PhyGen.Application.Notification.Exceptions;
using PhyGen.Application.Notification.Queries;
using PhyGen.Application.Notification.Responses;
using PhyGen.Domain.Entities;
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
        private readonly IMapper _mapper;

        public GetAllNotificationQueryHandle(INotificationRepository notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public async Task<List<NotificationResponse>> Handle(GetAllNotificationQuery request, CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.GetByUserIdAsync(request.Id);
            if (notification == null)
            {
                // Xử lý nếu không tìm thấy
                throw new NotificationNotFoundException();
            }
  
            return AppMapper<CoreMappingProfile>.Mapper
                    .Map<List<NotificationResponse>>(notification);
        }
    }

}
