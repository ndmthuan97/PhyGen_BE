using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using PhyGen.Insfrastructure.Persistence.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Insfrastructure.Persistence.Repositories
{
    public class MatrixContentItemRepository : RepositoryBase<MatrixContentItem, int>, IMatrixContentItemRepository
    {
        public MatrixContentItemRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<MatrixContentItem?> GetByMatrixIdAndContentItemIdAsync(Guid matrixId, Guid contentItemId)
        {
            return await _context.Set<MatrixContentItem>()
                .FirstOrDefaultAsync(mci => mci.MatrixId == matrixId && mci.ContentItemId == contentItemId);
        }
    }
}
