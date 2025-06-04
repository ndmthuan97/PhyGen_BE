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
    public class MatrixRepository : RepositoryBase<Matrix, Guid>, IMatrixRepository
    {
        public MatrixRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Matrix?> GetMatrixByUserIdAsync(Guid userId)
        {
            return await _context.Matrices
                .FirstOrDefaultAsync(a => a.UserId == userId);
        }
    } 
}
