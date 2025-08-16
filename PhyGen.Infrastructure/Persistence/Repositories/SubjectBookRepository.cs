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
    public class SubjectBookRepository : RepositoryBase<SubjectBook, Guid>, ISubjectBookRepository
    {
        public SubjectBookRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Pagination<SubjectBook>?> GetSubjectBooksBySubjectWithSpecAsync(SubjectBookSpecParam subjectBookSpecParam)
        {
            var spec = new SubjectBookSpecification(subjectBookSpecParam);
            return await GetWithSpecAsync(spec);
        }

        public async Task<List<SubjectBook>> GetSubjectBooksByGradeAsync(int grade)
        {
            return await _context.SubjectBooks
                .Where(sb => sb.Grade == grade)
                .ToListAsync();
        }

        public async Task<object?> GetNamesByTopicIdAsync(Guid topicId)
        {
            return await _context.Topics
                .Where(t => t.Id == topicId &&
                            t.DeletedAt == null &&
                            t.Chapter != null && t.Chapter.DeletedAt == null &&
                            t.Chapter.SubjectBook != null && t.Chapter.SubjectBook.DeletedAt == null)
                .Select(t => new
                {
                    TopicName = t.Name,
                    ChapterName = t.Chapter.Name,
                    SubjectBookName = t.Chapter.SubjectBook.Name
                })
                .FirstOrDefaultAsync();
        }
    }
}
