using PhyGen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IAsyncRepository<User, Guid>
    {
        Task<User?> GetUserByEmailAsync(string email);
    }
}
