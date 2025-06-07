using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class SubjectCurriculum : EntityBase<Guid>
    {
        public Guid SubjectId { get; set; }
        public Guid CurriculumId { get; set; }

        public virtual Subject Subject { get; set; } = null!;
        public virtual Curriculum Curriculum { get; set; } = null!;
        public virtual ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
        public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
    }
}
