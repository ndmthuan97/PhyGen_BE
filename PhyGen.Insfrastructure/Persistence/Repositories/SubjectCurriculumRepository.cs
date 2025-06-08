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
    public class SubjectCurriculumRepository : RepositoryBase<SubjectCurriculum, Guid>, ISubjectCurriculumRepository
    {
        public SubjectCurriculumRepository(AppDbContext context) : base(context)
        {
        }
    }
}
