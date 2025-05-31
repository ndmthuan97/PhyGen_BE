using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ChapterUnits.Commands
{
    public class DeleteChapterUnitCommand : IRequest<Unit>
    {
        public Guid ChapterUnitId { get; set; }
        public string DeletedBy { get; set; } = string.Empty;
    }
}
