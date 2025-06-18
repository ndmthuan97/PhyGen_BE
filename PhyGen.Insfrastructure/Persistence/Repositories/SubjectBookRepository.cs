using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using PhyGen.Domain.Specs;
using PhyGen.Domain.Specs.SubjectBooks;
using PhyGen.Infrastructure.Specifications.SubjectBooks;
using PhyGen.Insfrastructure.Persistence.DbContexts;
using PhyGen.Insfrastructure.Persistence.Repositories;
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

        public async Task<SubjectBook?> GetSubjectBookByNameAsync(string name)
        {
            return await _context.SubjectBooks
                .FirstOrDefaultAsync(sb => sb.Name.ToLower() == name.ToLower());
        }

        public async Task<Pagination<SubjectBook>?> GetSubjectBooksAsync(SubjectBookSpecParam subjectBookSpecParam)
        {
            var spec = new SubjectBookSpecification(subjectBookSpecParam);
            return await GetWithSpecAsync(spec);
        }

        public async Task<List<SubjectBook>> GetSubjectBooksBySubjectIdAsync(Guid subjectId)
        {
            return await _context.SubjectBooks
                .Where(sb => sb.SubjectId == subjectId)
                .ToListAsync();
        }

        public async Task<Pagination<SubjectBook>?> GetSubjectBooksBySubjectIdWithSpecAsync(SubjectBookBySubjectIdSpecParam subjectBookBySubjectIdSpecParam)
        {
            var spec = new SubjectBookBySubjectIdSpecification(subjectBookBySubjectIdSpecParam);
            return await GetWithSpecAsync(spec);
        }
    }
}
