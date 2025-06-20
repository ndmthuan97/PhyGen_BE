using PhyGen.Domain.Entities;
using PhyGen.Domain.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces
{
    public interface ICurriculumRepository : IAsyncRepository<Curriculum, Guid>
    {
        Task<Pagination<Curriculum>?> GetCurriculumsAsync(CurriculumSpecParam curriculumSpecParam);
    }
}
