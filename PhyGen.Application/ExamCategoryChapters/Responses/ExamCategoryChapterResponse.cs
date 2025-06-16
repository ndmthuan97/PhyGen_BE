using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ExamCategoryChapters.Responses
{
    public class ExamCategoryChapterResponse
    {
        public Guid Id { get; set; }
        public Guid ExamCategoryId { get; set; }
        public Guid ChapterId { get; set; }
    }
}
