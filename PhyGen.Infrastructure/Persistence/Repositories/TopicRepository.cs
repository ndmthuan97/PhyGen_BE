using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using PhyGen.Domain.Specs;
using PhyGen.Infrastructure.Specifications;
using PhyGen.Infrastructure.Persistence.DbContexts;
using PhyGen.Infrastructure.Persistence.Repositories;
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

        public async Task<Pagination<Topic>?> GetTopicsByChapterAsync(TopicSpecParam topicSpecParam)
        {
            var spec = new TopicSpecification(topicSpecParam);
            return await GetWithSpecAsync(spec);
        }

        public async Task<List<Topic>> GetValidTopicsAsync()
        {
            return await _context.Topics.Include(t => t.Chapter).ThenInclude(c => c.SubjectBook)
                .Where(t => t.DeletedAt == null &&
                            t.Chapter != null &&
                            t.Chapter.DeletedAt == null &&
                            t.Chapter.SubjectBook != null &&
                            t.Chapter.SubjectBook.DeletedAt == null)
                .ToListAsync();
        }
    }
}
