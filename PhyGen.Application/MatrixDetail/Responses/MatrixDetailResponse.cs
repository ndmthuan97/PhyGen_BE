using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixDetail.Responses
{
    public class MatrixDetailResponse
    {
        public Guid Id { get; set; }
        public Guid MatrixId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }     
        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
