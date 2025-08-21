using PhyGen.Domain.Entities;
using PhyGen.Domain.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces
{
    public interface IMatrixRepository : IAsyncRepository<Matrix, Guid>
    {
        Task<Pagination<Matrix>?> GetMatricesAsync(MatrixSpecParam matrixSpecParam);
    }
}
