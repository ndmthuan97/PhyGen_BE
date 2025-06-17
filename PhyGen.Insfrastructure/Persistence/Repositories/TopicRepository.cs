using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using PhyGen.Insfrastructure.Persistence.DbContexts;
using PhyGen.Insfrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Persistence.Repositories
{
    public class TopicRepository : RepositoryBase<Topic, Guid>, ITopicRepository
    {
        public TopicRepository(AppDbContext context) : base(context)
        {
        }

        public Task<Topic?> GetTopicByChapterIdAndNameAsync(Guid chapterId, string name)
        {
            return _context.Topics
                .FirstOrDefaultAsync(t => t.ChapterId == chapterId && t.Name.ToLower() == name.ToLower());
        }

        public async Task<List<Topic>> GetTopicsByChapterIdAsync(Guid chapterId)
        {
            return await _context.Topics
                .Where(t => t.ChapterId == chapterId)
                .ToListAsync();
        }
    }
}
