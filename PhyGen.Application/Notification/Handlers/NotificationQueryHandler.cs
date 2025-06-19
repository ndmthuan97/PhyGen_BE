using AutoMapper;
using MediatR;
using Org.BouncyCastle.Asn1.Ocsp;
using PhyGen.Application.Mapping;
using PhyGen.Application.Notification.Exceptions;
using PhyGen.Application.Notification.Queries;
using PhyGen.Application.Notification.Responses;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using PhyGen.Domain.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Exams.Handlers
{
    public class GetAllNotificationQueryHandle : IRequestHandler<GetAllNotificationQuery, Pagination<NotificationResponse>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public GetAllNotificationQueryHandle(INotificationRepository notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public async Task<Pagination<NotificationResponse>> Handle(GetAllNotificationQuery request, CancellationToken cancellationToken)
        {
            int skip = (request.PageIndex - 1) * request.PageSize;

            var (notifications, totalCount) = await _notificationRepository.GetByUserIdAsync(request.Id, skip, request.PageSize);

            var data = _mapper.Map<List<NotificationResponse>>(notifications);

            return new Pagination<NotificationResponse>(
                request.PageIndex,
                request.PageSize,
                totalCount,
                data
            );
        }
    }
}
