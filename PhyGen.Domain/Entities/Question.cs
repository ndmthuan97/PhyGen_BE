using PhyGen.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class Question : EntityBase<Guid>
    {
        [Required]
        public Guid TopicId { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty; // Nội dung câu hỏi

        public QuestionType Type { get; set; } // Loại câu hỏi (MultipleChoice, TrueFalse, ShortAnswer, Essay)
        public DifficultyLevel Level { get; set; } // Mức độ

        public string? Image { get; set; } // Hình ảnh minh họa (nếu có)

        public string? Answer1 { get; set; } // Đáp án 1 
        public string? Answer2 { get; set; } // Đáp án 2
        public string? Answer3 { get; set; } // Đáp án 3
        public string? Answer4 { get; set; } // Đáp án 4
        public string? Answer5 { get; set; } // Đáp án 5
        public string? Answer6 { get; set; } // Đáp án 6

        public string? CorrectAnswer { get; set; } // Đáp án đúng (A, B, true, hoặc text)

        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }

        // --- Navigation Properties ---
        public virtual Topic Topic { get; set; } = default!;
        public virtual ICollection<QuestionMedia> QuestionMedias { get; set; } = new List<QuestionMedia>();
        public virtual ICollection<QuestionSection> QuestionSections { get; set; } = new List<QuestionSection>();
    }
}
