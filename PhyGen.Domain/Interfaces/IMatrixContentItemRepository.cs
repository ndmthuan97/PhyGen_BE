using PhyGen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces
{
    public interface IMatrixContentItemRepository : IAsyncRepository<MatrixContentItem, int>
    {
        Task<MatrixContentItem?> GetByMatrixIdAndContentItemIdAsync(Guid matrixId, Guid contentItemId);
    }
}
