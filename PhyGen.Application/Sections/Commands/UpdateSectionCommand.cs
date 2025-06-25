using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Sections.Commands
{
    public class UpdateSectionCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public Guid ExamId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string SectionType { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
}
