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
    public class MatrixDetailRepository : RepositoryBase<MatrixDetail, Guid>, IMatrixDetailRepository
    {
        public MatrixDetailRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<MatrixDetail?> GetMatrixDetailByMatrixIdAsync(Guid matrixId)
        {
            return await _context.MatrixDetails
                .FirstOrDefaultAsync(a => a.MatrixId == matrixId);
        }

        public async Task<MatrixDetail?> GetMatrixDetailByChapterIdAsync(Guid chapterId)
        {
            return await _context.MatrixDetails
                .FirstOrDefaultAsync(a => a.ChapterId == chapterId);
        }
    }
}
