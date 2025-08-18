using PhyGen.Shared.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateMatrixRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid SubjectId { get; set; }

        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid ExamCategoryId { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Tên ma trận không được vượt quá 255 ký tự.")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        public int TotalQuestionCount { get; set; }

        [Required(ErrorMessage = "Trường này không được để trống.")]
        [Range(10, 12, ErrorMessage = "Lớp học phải nằm trong khoảng 10 đến 12.")]
        public int Grade { get; set; }
        public int Year { get; set; }
        public string? ImgUrl { get; set; } = string.Empty;

        public StatusQEM Status { get; set; }
    }

    public class UpdateMatrixRequest
    {
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid SubjectId { get; set; }

        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid ExamCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        public int TotalQuestionCount { get; set; }

        [Required(ErrorMessage = "Trường này không được để trống.")]
        [Range(10, 12, ErrorMessage = "Lớp học phải nằm trong khoảng 10 đến 12.")]
        public int Grade { get; set; }
        public int Year { get; set; }
        public string? ImgUrl { get; set; } = string.Empty;

        public StatusQEM Status { get; set; }
    }

    public class DeleteMatrixRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid Id { get; set; }
    }

    public class UpdateMatrixStatusRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid Id { get; set; }

        public StatusQEM Status { get; set; }
    }
}
