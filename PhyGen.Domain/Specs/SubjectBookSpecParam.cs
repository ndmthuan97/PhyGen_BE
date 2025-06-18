using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhyGen.Domain.Specs
{
    public class SubjectBookSpecParam : BaseSpecParam
    {
        [JsonIgnore]
        public Guid SubjectId { get; set; }
        public string? Search { get; set; }
        public string? Sort { get; set; }
    }

    public class SubjectBookBySubjectSpecParam : BaseSpecParam
    {
        public Guid SubjectId { get; set; }
    }
}
