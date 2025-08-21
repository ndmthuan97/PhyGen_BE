using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.QuestionSections.Responses
{
    public class QuestionSectionResponse
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public Guid SectionId { get; set; }
        public double? Score { get; set; }  
    }
}
