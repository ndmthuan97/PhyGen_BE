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
    public class MatrixContentItemRepository : RepositoryBase<MatrixContentItem, Guid>, IMatrixContentItemRepository
    {
        public MatrixContentItemRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<MatrixContentItem?> GetMatrixContentItemsByMatrixIdAndContentItemIdAsync(Guid matrixId, Guid contentItemId)
        {
            return await _context.MatrixContentItems
                .FirstOrDefaultAsync(mci => mci.MatrixId == matrixId && mci.ContentItemId == contentItemId);
        }
    }
}
