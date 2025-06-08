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
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid SubjectCurriculumId { get; set; }
        public int? OrderNo { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
    }
}
