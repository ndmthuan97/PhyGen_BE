using AutoMapper;
using MediatR;
using PhyGen.Application.Mapping;
using PhyGen.Application.Notification.Commands;
using PhyGen.Application.Notification.Exceptions;
using PhyGen.Application.Notification.Responses;
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
        private readonly IUserRepository _userRepository;

        public CreateNotificationCommandHandler(
            INotificationRepository notificationRepository,
            IUserRepository userRepository)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
        }
        public async Task<NotificationResponse> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            // Nếu UserId được chỉ định, tạo thông báo cho một người dùng
            if (request.UserId != null)
            {
                var notification = new PhyGen.Domain.Entities.Notification
                {
                    UserId = request.UserId,
                    Title = request.Title,
                    Message = request.Message,
                    CreatedAt = request.CreatedAt
                };

                await _notificationRepository.AddAsync(notification);
                return AppMapper<CoreMappingProfile>.Mapper.Map<NotificationResponse>(notification);
            }

            var allUsers = await _userRepository.GetAllAsync();

            var notifications = allUsers.Select(user => new PhyGen.Domain.Entities.Notification
            {
                UserId = user.Id,
                Title = request.Title,
                Message = request.Message,
                CreatedAt = request.CreatedAt
            }).ToList();

            foreach (var notification in notifications)
            {
                await _notificationRepository.AddAsync(notification);
            }

            return AppMapper<CoreMappingProfile>.Mapper.Map<NotificationResponse>(notifications.First());
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
}
