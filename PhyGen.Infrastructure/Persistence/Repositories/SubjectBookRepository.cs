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
    }
}
