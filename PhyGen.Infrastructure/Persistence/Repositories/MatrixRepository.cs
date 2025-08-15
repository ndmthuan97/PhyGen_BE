using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using PhyGen.Domain.Specs;
using PhyGen.Infrastructure.Persistence.DbContexts;
using PhyGen.Infrastructure.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Persistence.Repositories
{
    public class MatrixRepository : RepositoryBase<Matrix, Guid>, IMatrixRepository
    {
        public MatrixRepository(AppDbContext context) : base(context) { }

        public async Task<Pagination<Matrix>?> GetMatricesAsync(MatrixSpecParam matrixSpecParam)
        {
            var spec = new MatrixSpecification(matrixSpecParam);
            return await GetWithSpecAsync(spec);
        }

        public async Task<string> GenerateMatrixCodeAsync()
        {
            var last = await _context.Matrices
                .Where(m => !string.IsNullOrEmpty(m.MatrixCode) && m.MatrixCode.StartsWith("M"))
                .OrderByDescending(m => m.CreatedAt)
                .Select(m => m.MatrixCode)
                .FirstOrDefaultAsync();

            int next = 1;
            if (!string.IsNullOrEmpty(last))
            {
                var digits = last.Substring(1);
                if (int.TryParse(digits, out var n))
                    next = n + 1;
            }

            return $"M{next:D3}";
        }
    }
}
