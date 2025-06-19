using PhyGen.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class Section : EntityBase<Guid>
    {
        [Required]
        public Guid ExamId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = default!; // Tên phần (VD: "Phần trắc nghiệm ABCD")

        public string? Description { get; set; } // Mô tả nếu cần
        public QuestionType SectionType { get; set; } // Loại phần (MultipleChoice, TrueFalse, ShortAnswer, Essay)
        public int DisplayOrder { get; set; } // Thứ tự hiển thị của phần trong Exam

        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }

        // --- Navigation Properties ---
        public virtual Exam Exam { get; set; } = default!;
        public virtual ICollection<QuestionSection> QuestionSections { get; set; } = new List<QuestionSection>();
    }
}
