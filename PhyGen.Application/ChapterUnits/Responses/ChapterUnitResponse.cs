using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ChapterUnits.Responses
{
    public class ChapterUnitResponse
    {
        public Guid ChapterUnitId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int? OrderNo { get; set; }

        public Guid ChapterId { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? DeletedBy { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
