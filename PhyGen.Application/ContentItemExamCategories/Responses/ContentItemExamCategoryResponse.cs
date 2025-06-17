using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.ContentItemExamCategories.Responses
{
    public class ContentItemExamCategoryResponse
    {
        public Guid Id { get; set; }
        public Guid ContentItemId { get; set; }
        public Guid ExamCategoryId { get; set; }
    }
}
