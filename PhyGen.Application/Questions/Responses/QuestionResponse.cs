using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Questions.Responses
{
    public class QuestionResponse
    {
        public Guid Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public string? Type { get; set; }

        public string? Level { get; set; }

        public string? Image { get; set; }

        public Guid ChapterId { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? DeletedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        //public string? Answer { get; set; } = string.Empty;
    }
}
