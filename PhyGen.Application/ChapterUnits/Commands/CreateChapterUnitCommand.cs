using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ChapterUnits.Commands
{
    public class CreateChapterUnitCommand : IRequest<Guid>
    {
        public Guid ChapterId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int? OrderNo { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
    }
}
