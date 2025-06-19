using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class MatrixSectionDetail : EntityBase<Guid>
    {
        [Required]
        public Guid MatrixSectionId { get; set; }

        [Required]
        public Guid SectionId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = default!; // Tên chi tiết chương (VD: "Vật lí nhiệt")

        public string? Description { get; set; } // Mô tả nếu cần

        public int Quantity { get; set; } // Số lượng câu hỏi
        public string Level { get; set; } = default!; // VD: "Nhận biết", "Thông hiểu", "Vận dụng" (có thể đặt Enum nếu muốn kiểm soát chặt hơn)

        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }

        // --- Navigation Properties ---
        public virtual MatrixSection MatrixSection { get; set; } = default!;
        public virtual Section Section { get; set; } = null!;
    }
}
