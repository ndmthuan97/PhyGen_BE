using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class Subject : EntityBase<Guid>
    {
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<Matrix> Matrices { get; set; } = new List<Matrix>();
        public virtual ICollection<SubjectBook> SubjectBooks { get; set; } = new List<SubjectBook>();
    }
}
