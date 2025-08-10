using Microsoft.EntityFrameworkCore;
using PhyGen.Infrastructure.Persistence.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Service
{
    public class TopicService
    {
        private readonly AppDbContext _dbContext;
        public TopicService(AppDbContext dbContext) => _dbContext = dbContext;

        public async Task<Guid?> GetTopicIdByNameAsync(string topicName)
        {
            if (string.IsNullOrWhiteSpace(topicName)) return null;

            var normalized = topicName.Trim().ToLower();
            var topic = await _dbContext.Topics
                .FirstOrDefaultAsync(t => t.Name.Trim().ToLower() == normalized);

            return topic?.Id;
        }
    }

}
