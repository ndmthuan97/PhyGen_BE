using PhyGen.Domain.Entities;
using PhyGen.Domain.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces
{
    public interface IMatrixSectionRepository : IAsyncRepository<MatrixSection, Guid>
    {
        Task<Pagination<MatrixSection>?> GetMatrixSectionsByMatrixIdAsync(MatrixSectionSpecParam param);
    }
}
