using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ChapterUnits.Commands
{
    public class UpdateChapterUnitCommand : IRequest<Unit>
    {
        public Guid ChapterUnitId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? OrderNo { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public Guid ChapterId { get; set; }
    }
}
