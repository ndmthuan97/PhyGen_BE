using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Chapters.Commands
{
    public class CreateChapterCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;

        public Guid SubjectCurriculumId { get; set; }

        public int? OrderNo { get; set; }

        public string? CreatedBy { get; set; }
    }
}
