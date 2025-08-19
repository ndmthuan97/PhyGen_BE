using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using PhyGen.Infrastructure.Persistence.DbContexts;
using PhyGen.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Persistence.Repositories
{
    public class ContentFlowRepository : RepositoryBase<ContentFlow, Guid>, IContentFlowRepository
    {
        public ContentFlowRepository(AppDbContext context) : base(context)
        {
        }

        public Task<List<ContentFlow>> GetContentFlowsByCurriculumIdAndSubjectIdAsync(Guid curriculumId, Guid subjectId)
        {
            return _context.ContentFlows
                .Where(cf => cf.CurriculumId == curriculumId && cf.SubjectId == subjectId && cf.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<int> GetMaxOrderNoByCurriculumIdAndSubjectIdAsync(Guid curriculumId, Guid subjectId, int grade)
        {
            return await _context.ContentFlows
                .Where(cf => cf.CurriculumId == curriculumId && cf.SubjectId == subjectId && cf.DeletedAt == null && grade == cf.Grade)
                .Select(cf => (int?)cf.OrderNo)
                .MaxAsync() ?? 0;
        }
    }
}
