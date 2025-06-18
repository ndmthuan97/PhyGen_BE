using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using PhyGen.Domain.Specs;
using PhyGen.Infrastructure.Specifications;
using PhyGen.Insfrastructure.Persistence.DbContexts;
using PhyGen.Insfrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Persistence.Repositories
{
    public class ChapterRepository : RepositoryBase<Chapter, Guid>, IChapterRepository
    {
        public ChapterRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Pagination<Chapter>?> GetChaptersBySubjectBookAsync(ChapterSpecParam chapterSpecParam)
        {
            var spec = new ChapterSpecification(chapterSpecParam);
            return await GetWithSpecAsync(spec);
        }
    }
}
