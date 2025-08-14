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
using PhyGen.Domain.Specs.Topic;
using PhyGen.Infrastructure.Specifications.Topics;

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

        public async Task<int?> GetGradeByTopicIdAsync(Guid id)
        {
            return await _context.Topics.Include(t => t.Chapter).ThenInclude(c => c.SubjectBook)
                .Where(t => t.Id == id && t.DeletedAt == null)
                .Select(t => t.Chapter.SubjectBook.Grade)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Topic>> GetTopicsByGradeAsync(TopicByGradeSpecParam topicByGradeSpecParam)
        {
            var spec = new TopicByGradeSpecification(topicByGradeSpecParam);
            return await ListWithSpecAsync(spec);
        }

        public async Task<string> GetTopicCodeAsync()
        {
            var lastTopic = await _context.Topics
                .OrderByDescending(t => t.TopicCode)
                .FirstOrDefaultAsync();
            if (lastTopic == null || string.IsNullOrEmpty(lastTopic.TopicCode))
            {
                return "T0001";
            }
            if (!int.TryParse(lastTopic.TopicCode.Substring(1), out int lastNumber))
            {
                lastNumber = 0;
            }
            int newNumber = lastNumber + 1;
            return $"T{newNumber:D4}";
        }
    }
}
