using PhyGen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Interfaces.Repositories
{
    public interface IMatrixDetailRepository : IAsyncRepository<MatrixDetail, Guid>
    {
        Task <MatrixDetail?> GetMatrixDetailByMatrixIdAsync(Guid matrixId);
        Task<MatrixDetail?> GetMatrixDetailByChapterIdAsync(Guid chapterId);
    }
}
