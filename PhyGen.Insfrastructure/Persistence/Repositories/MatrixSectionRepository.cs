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
    public class MatrixSectionRepository : RepositoryBase<MatrixSection, Guid>, IMatrixSectionRepository
    {
        public MatrixSectionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<MatrixSection>?> GetMatrixSectionsByMatrixIdAsync(Guid matrixId)
        {
            return await _context.MatrixSections
                .Where(ms => ms.MatrixId == matrixId && !ms.DeletedAt.HasValue)
                .ToListAsync();
        }
    }
}
