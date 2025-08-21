using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using PhyGen.Domain.Specs;
using PhyGen.Infrastructure.Specifications;
using PhyGen.Infrastructure.Persistence.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Persistence.Repositories
{
    public class CurriculumRepository : RepositoryBase<Curriculum, Guid>, ICurriculumRepository
    {
        public CurriculumRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<Pagination<Curriculum>?> GetCurriculumsAsync(CurriculumSpecParam curriculumSpecParam)
        {
            var spec = new CurriculumSpecification(curriculumSpecParam);
            return await GetWithSpecAsync(spec);
        }
    }
}
