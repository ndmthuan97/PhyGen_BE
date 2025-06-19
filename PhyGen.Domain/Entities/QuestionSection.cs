using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class QuestionSection : EntityBase<Guid>
    {
        [Required]
        public Guid SectionId { get; set; }
        [Required]
        public Guid QuestionId { get; set; }

        public double? Score { get; set; } // Điểm số của câu hỏi trong phần

        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }

        // --- Navigation Properties ---
        public virtual Section Section { get; set; } = default!;
        public virtual Question Question { get; set; } = default!;

        //** --- Comment các thuộc tính có thể dùng trong tương lai (hiện tại không dùng đến) ---
        // public int Order { get; set; } // Thứ tự ban đầu (dựa trên MatrixSectionDetail)
        // public int DisplayOrder { get; set; } // Thứ tự hiển thị sau xáo trộn
        //**
    }
}
