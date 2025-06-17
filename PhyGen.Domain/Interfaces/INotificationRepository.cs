using PhyGen.Domain.Entities;

namespace PhyGen.Domain.Interfaces
{
    public interface INotificationRepository : IAsyncRepository<Notification, int>
    {
        Task<List<Notification>> GetByUserIdAsync(Guid id);
    }
}
