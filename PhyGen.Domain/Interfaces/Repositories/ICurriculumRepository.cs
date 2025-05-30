using PhyGen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces.Repositories
{
    public interface ICurriculumRepository : IAsyncRepository<Curriculum, Guid>
    {
        Task<Curriculum?> GetCurriculumByNameAsync(string curriculumName);
    }
}
