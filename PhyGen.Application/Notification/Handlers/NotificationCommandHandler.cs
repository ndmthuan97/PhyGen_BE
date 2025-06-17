using AutoMapper;
using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.Matrices.Exceptions;
using PhyGen.Application.Notification.Commands;
using PhyGen.Application.Notification.Exceptions;
using PhyGen.Application.Notification.Responses;
using PhyGen.Application.SubjectCurriculums.Exceptions;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Notification.Handlers
{
    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, NotificationResponse>
    {
        private readonly INotificationRepository _notificationRepository;

        public CreateNotificationCommandHandler(
            INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task<NotificationResponse> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = new PhyGen.Domain.Entities.Notification
            {             
                Title = request.Title,
                Message = request.Message,       
                CreatedAt = request.CreatedAt
            };

            await _notificationRepository.AddAsync(notification);

            return AppMapper<CoreMappingProfile>.Mapper.Map<NotificationResponse>(notification);
        }
    }

    public class UpdateNotificationCommandHandler : IRequestHandler<UpdateNotificationCommand, Unit>
    {
        private readonly INotificationRepository _notificationRepository;

        public UpdateNotificationCommandHandler(
            INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task<Unit> Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.GetByIdAsync(request.Id);
            if (notification == null)
                throw new NotificationNotFoundException();

            notification.Title = request.Title;
            notification.Message = request.Message;

            await _notificationRepository.UpdateAsync(notification);
            return Unit.Value;
        }
    }
    public class UpdateNotificationIsReadCommandHandler : IRequestHandler<UpdateNotificationReadCommand, Unit>
    {
        private readonly INotificationRepository _notificationRepository;

        public UpdateNotificationIsReadCommandHandler(
            INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task<Unit> Handle(UpdateNotificationReadCommand request, CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.GetByUserIdAsync(request.UserId);
            if (notification == null)
                throw new NotificationNotFoundException();

            foreach (var notifications in notification)
            {
                notifications.IsRead = true;
                await _notificationRepository.UpdateAsync(notifications);
            }
            return Unit.Value;
        }
    }

    public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand, Unit>
    {
        private readonly INotificationRepository _notificationRepository;

        public DeleteNotificationCommandHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task<Unit> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.GetByIdAsync(request.Id);
            if (notification == null)
                throw new NotificationNotFoundException();

            await _notificationRepository.DeleteAsync(notification);
            return Unit.Value;
        }
    }
    public class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand, List<NotificationResponse>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public SendNotificationCommandHandler(INotificationRepository notificationRepository, IUserRepository userRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<NotificationResponse>> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
        {
            // Lấy thông tin Notification mẫu
            var originalNotification = await _notificationRepository.GetByIdAsync(request.Id);

            if (originalNotification == null)
                throw new Exception($"Notification with ID {request.Id} not found.");

            var notifications = new List<PhyGen.Domain.Entities.Notification>();

            if (!request.UserId.HasValue)
            {
                // Gửi cho tất cả người dùng
                var allUsers = await _userRepository.GetAllAsync();

                foreach (var user in allUsers)
                {
                    notifications.Add(new PhyGen.Domain.Entities.Notification
                    {
                        Title = originalNotification.Title,
                        Message = originalNotification.Message,
                        CreatedAt = DateTime.UtcNow,
                        UserId = user.Id
                    });
                }
            }
            else
            {
                // Gửi riêng cho 1 người dùng cụ thể
                notifications.Add(new PhyGen.Domain.Entities.Notification
                {
                    Title = originalNotification.Title,
                    Message = originalNotification.Message,
                    CreatedAt = DateTime.UtcNow,
                    UserId = request.UserId.Value
                });
            }

            // Lưu vào DB
            foreach (var notification in notifications)
            {
                await _notificationRepository.AddAsync(notification);
            }

            return _mapper.Map<List<NotificationResponse>>(notifications);
        }

    }
}
