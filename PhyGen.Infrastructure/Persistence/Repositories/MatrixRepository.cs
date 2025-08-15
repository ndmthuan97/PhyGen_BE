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
    }
}
