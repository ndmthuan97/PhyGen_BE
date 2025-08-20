using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixSections.Responses
{
    public class MatrixSectionResponse
    {
        public Guid Id { get; set; }
        public Guid MatrixId { get; set; }
        public string Title { get; set; } = string.Empty;
        public float? Score { get; set; }
        public string? Description { get; set; }
    }
}
