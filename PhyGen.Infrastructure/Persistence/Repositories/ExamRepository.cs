using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using PhyGen.Domain.Specs;
using PhyGen.Infrastructure.Persistence.DbContexts;
using PhyGen.Infrastructure.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Persistence.Repositories
{
    public class ExamRepository : RepositoryBase<Exam, Guid>, IExamRepository
    {
        public ExamRepository(AppDbContext context) : base(context) { }

        public async Task<Pagination<Exam>?> GetExamsAsync(ExamSpecParam examSpecParam)
        {
            var spec = new ExamSpecification(examSpecParam);
            return await GetWithSpecAsync(spec);
        }
    }
}
