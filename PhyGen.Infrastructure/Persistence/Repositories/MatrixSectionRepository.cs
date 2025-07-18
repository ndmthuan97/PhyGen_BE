using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using PhyGen.Domain.Specs;
using PhyGen.Infrastructure.Persistence.DbContexts;
using PhyGen.Infrastructure.Persistence.Repositories;
using PhyGen.Infrastructure.Specifications;
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

        public async Task<Pagination<MatrixSection>?> GetMatrixSectionsByMatrixIdAsync(MatrixSectionSpecParam param)
        {
            var spec = new MatrixSectionSpecification(param);
            return await GetWithSpecAsync(spec);
        }
    }
}
