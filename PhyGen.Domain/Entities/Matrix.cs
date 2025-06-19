using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class Matrix : EntityBase<Guid>
    {
        [Required]
        public Guid SubjectId { get; set; }

        [Required]
        public Guid ExamCategoryId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = default!; // Tên ma trận (VD: Ma trận đề Giữa kỳ lớp 10)

        public string? Description { get; set; } // Mô tả ma trận
        public int TotalQuestionCount { get; set; } // Tổng số câu hỏi
        public int Grade { get; set; } // Lớp (10, 11, 12)
        public DateTime? Year { get; set; } // Năm học áp dụng (VD: 2025)

        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }


        // --- Navigation Properties ---
        public virtual Subject Subject { get; set; } = null!;
        public virtual ExamCategory ExamCategory { get; set; } = null!;
        public virtual ICollection<MatrixContentItem> MatrixContentItems { get; set; } = new List<MatrixContentItem>();
        public virtual ICollection<MatrixSection> MatrixSections { get; set; } = new List<MatrixSection>();
        public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
        public virtual User Creator { get; set; } = default!;

        //** --- Comment các thuộc tính có thể dùng trong tương lai (hiện tại không dùng đến) ---
        // public string Type { get; set; } // Loại ma trận (Tự tạo, từ hệ thống, từ AI,...)
        // public bool IsPublic { get; set; } // Có thể chia sẻ không
        // public string DifficultyDistribution { get; set; } // Phân bổ độ khó (đã bỏ)
        // public string QuestionTypeRatio { get; set; } // Tỷ lệ loại câu hỏi (đã bỏ)
        // public int? DurationMinutes { get; set; } // Thời gian làm bài (đã bỏ)
        // public double? TotalScore { get; set; } // Tổng điểm (đã bỏ)
        //**
    }
}
