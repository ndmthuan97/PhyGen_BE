using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class Section : EntityBase<Guid>
    {
        public Guid ExamId { get; set; }

        public virtual Exam Exam { get; set; } = null!;
        public virtual ICollection<QuestionSection> QuestionSections { get; set; } = new List<QuestionSection>();
        public virtual ICollection<MatrixSectionDetail> MatrixSectionDetails { get; set; } = new List<MatrixSectionDetail>();
    }
}
