using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Chapters.Responses
{
    public class ChapterResponse
    {
        public Guid Id { get; set; }
        public Guid SubjectBookId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ChapterCode { get; set; } = string.Empty;
    }
}
