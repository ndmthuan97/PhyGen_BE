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
    public class SubjectRepository : RepositoryBase<Subject, Guid>, ISubjectRepository
    {
        public SubjectRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Subject?> GetSubjectByNameAsync(string name)
        {
            return await _context.Subjects
                .FirstOrDefaultAsync(s => s.Name.ToLower() == name.ToLower());
        }
    }
}
