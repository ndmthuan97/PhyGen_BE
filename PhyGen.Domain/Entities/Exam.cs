using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Entities
{
    public class Exam : EntityBase<Guid>
    {
        [Required]
        public Guid ExamCategoryId { get; set; }
        [Required]
        public Guid MatrixId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty; // Tên bài thi (VD: Đề kiểm tra giữa kỳ lớp 10)
        public string? Description { get; set; } // Mô tả bài thi

        public int Grade { get; set; } // Lớp (10, 11, 12)
        public DateTime? Year { get; set; } // Năm học
        public int? TotalQuestionCount { get; set; } // Tổng số câu hỏi

        public int VersionCount { get; set; } // Số lượng đề cần sinh
        public bool RandomizeQuestions { get; set; } // Có xáo trộn câu hỏi không

        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }

        // --- Navigation Properties ---
        public virtual ExamCategory ExamCategory { get; set; } = default!;
        public virtual Matrix Matrix { get; set; } = default!;
        public virtual ICollection<Section> Sections { get; set; } = new List<Section>(); // Thay Questions bằng Sections
        public virtual ICollection<ExamVersion> ExamVersions { get; set; } = new List<ExamVersion>();

        //** --- Comment các thuộc tính có thể dùng trong tương lai (hiện tại không dùng đến) ---
        // public Guid SubjectId { get; set; } // Môn học nhưng tạm fix là môn Vật lý
        // public bool IsPublic { get; set; } // Có được chia sẻ công khai không
        // public string ExamType { get; set; } // Kiểu đề (Tự luận, Trắc nghiệm, Kết hợp)
        // public bool IsAIGenerated { get; set; } // Đề có được tạo bởi AI không
        // public string? PromptTemplate { get; set; } // Prompt dùng để tạo đề
        // public string Status { get; set; } // Nháp, Đã tạo, Đã duyệt, Công khai
        // public bool IsUsed { get; set; } // Đã dùng trong kỳ thi nào chưa
        // public bool RandomizeWithinSections { get; set; } // Xáo trộn trong từng Section (mặc định true), dùng thuộc tính này thì bỏ QuestionOrder trong ExamVersion
        // public int? DurationMinutes { get; set; } // Thời gian làm bài
        //**
    }
}
