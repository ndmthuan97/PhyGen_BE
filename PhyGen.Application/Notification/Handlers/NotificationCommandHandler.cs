using MediatR;
using PhyGen.Application.Matrices.Exceptions;
using PhyGen.Application.Notification.Commands;
using PhyGen.Application.Notification.Exceptions;
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
    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Guid>
    {
        private readonly INotificationRepository _notificationRepository;

        public CreateNotificationCommandHandler(
            INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<Guid> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = new PhyGen.Domain.Entities.Notification
            {             
                Title = request.Title,
                Message = request.Message,
                UserId = request.UserId,          
                CreatedAt = request.CreatedAt
            };

            await _notificationRepository.AddAsync(notification);

            return notification.UserId;           
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
