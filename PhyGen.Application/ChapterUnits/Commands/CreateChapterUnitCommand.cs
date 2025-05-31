using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ChapterUnits.Commands
{
    public class CreateChapterUnitCommand : IRequest<Guid>
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? OrderNo { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public Guid ChapterId { get; set; }
    }
}
