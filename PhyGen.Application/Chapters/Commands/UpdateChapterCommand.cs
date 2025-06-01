using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Chapters.Commands
{
    public class UpdateChapterCommand : IRequest<Unit>
    {
        public Guid ChapterId { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid? CurriculumId { get; set; }
        public Guid? BookId { get; set; }
        public int? OrderNo { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
    }
}
