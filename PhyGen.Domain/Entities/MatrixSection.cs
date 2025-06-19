using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class MatrixSection : EntityBase<Guid>
    {
        [Required]
        public Guid MatrixId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = default!; // VD: "Phần 1"

        public double? Score { get; set; } // Điểm số của câu hỏi trong phần

        public string? Description { get; set; } // Ghi chú thêm nếu cần

        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }

        // --- Navigation Properties ---
        public virtual Matrix Matrix { get; set; } = default!;
        public virtual ICollection<MatrixSectionDetail> MatrixSectionDetails { get; set; } = new List<MatrixSectionDetail>();

        //** --- Comment các thuộc tính có thể dùng trong tương lai (hiện tại không dùng đến) ---
        // public int DisplayOrder { get; set; } // Thứ tự hiển thị của phần trong ma trận
        //**
    }

}
