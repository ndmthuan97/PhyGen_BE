using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using PhyGen.Infrastructure.Persistence.DbContexts;
using PhyGen.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Persistence.Repositories
{
    public class MatrixSectionDetailRepository : RepositoryBase<MatrixSectionDetail, Guid>, IMatrixSectionDetailRepository
    {
        public MatrixSectionDetailRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<MatrixSectionDetail>?> GetMatrixSectionDetailsByMatrixSectionIdAsync(Guid matrixSectionId)
        {
            return await _context.MatrixSectionDetails
                .Where(msd => msd.MatrixSectionId == matrixSectionId && !msd.DeletedAt.HasValue)
                .ToListAsync();
        }
    }
}
