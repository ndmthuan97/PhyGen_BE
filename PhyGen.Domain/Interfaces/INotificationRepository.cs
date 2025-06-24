using PhyGen.Domain.Entities;

namespace PhyGen.Domain.Interfaces
{
    public interface INotificationRepository : IAsyncRepository<Notification, int>
    {
        Task<(List<Notification>, int)> GetByUserIdAsync(Guid userId, int skip, int take);
        Task<List<Notification>> GetByUserIdAsync(Guid? userId);
    }
}
