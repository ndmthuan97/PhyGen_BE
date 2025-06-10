using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Interfaces;
using PhyGen.Insfrastructure.Persistence.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhyGen.Insfrastructure.Persistence.Repositories
{
    public class ExamRepository : RepositoryBase<Exam, Guid>, IExamRepository
    {
        public ExamRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Exam?> GetByCreatorAsync(string createdBy)
        {
            return await _context.Set<Exam>()
                .FirstOrDefaultAsync(e => e.CreatedBy == createdBy);
        }

        public async Task<Exam?> GetBySubjectCurriculumIdOrMatrixIdAsync(Guid subjectCurriculumId, Guid matrixId)
        {
            return await _context.Set<Exam>()
                .FirstOrDefaultAsync(e => e.SubjectCurriculumId == subjectCurriculumId || e.MatrixId == matrixId);
        }
    }
}
