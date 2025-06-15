using PhyGen.Domain.Entities;

namespace PhyGen.Domain.Interfaces
{
    public interface INotificationRepository : IAsyncRepository<Notification, int>
    {
    }
}
