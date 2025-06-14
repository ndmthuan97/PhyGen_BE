using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
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

        public async Task<Matrix?> GetBySubjectCurriculumIdAndExamCategoryIdAsync(Guid subjectCurriculumId, Guid examCategoryId)
        {
            return await _context.Set<Matrix>()
                .FirstOrDefaultAsync(m => m.SubjectId == subjectCurriculumId && m.ExamCategoryId == examCategoryId);
        }


    }
}
