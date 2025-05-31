using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Chapters.Commands
{
    public class DeleteChapterCommand : IRequest<Unit>
    {
        public Guid ChapterId { get; set; }
        public string DeletedBy { get; set; } = string.Empty;
    }
}
