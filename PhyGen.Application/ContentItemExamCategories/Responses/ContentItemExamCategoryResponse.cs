using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentItemExamCategories.Responses
{
    public class ContentItemExamCategoryResponse
    {
        public int Id { get; set; }
        public Guid ContentItemId { get; set; }
        public int ExamCategoryId { get; set; }
    }
}
