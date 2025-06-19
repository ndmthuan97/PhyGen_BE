using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class ExamVersion : EntityBase<Guid>
    {
        [Required]
        public Guid ExamId { get; set; }

        [Required]
        public string Code { get; set; } = default!; // Mã đề (VD: "A1", "A2", "B1",...)

        // Danh sách ID câu hỏi theo thứ tự riêng biệt của version này. VD: "1cfa...,9bd3...,af88...,..." (chuỗi GUID cách nhau bởi dấu phẩy)
        public string QuestionOrder { get; set; } = default!;

        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }

        // --- Navigation Properties ---
        public virtual Exam Exam { get; set; } = default!;
    }
}
