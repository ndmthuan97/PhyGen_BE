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

        public Guid ChapterId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int? OrderNo { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public string UpdatedBy { get; set; } = string.Empty;
    }
}
