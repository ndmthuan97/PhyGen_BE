using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces.Repositories;
using PhyGen.Insfrastructure.Persistence.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Insfrastructure.Persistence.Repositories
{
    public class CurriculumRepository : RepositoryBase<Curriculum, Guid>, ICurriculumRepository
    {
        public CurriculumRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<Curriculum?> GetCurriculumByNameAsync(string curriculumName)
        {
            return await _context.Curriculums
                .FirstOrDefaultAsync(c =>
                    EF.Functions.Collate(c.Name, "Latin1_General_100_CI_AI_SC") == curriculumName);
        }
    }
}
