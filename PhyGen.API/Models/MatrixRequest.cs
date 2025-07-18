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
        public int Grade { get; set; }
        public int Year { get; set; }
        public string? ImgUrl { get; set; } = string.Empty;
    }

    public class UpdateMatrixRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid Id { get; set; }

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
        public int Grade { get; set; }
        public int Year { get; set; }
        public string? ImgUrl { get; set; } = string.Empty;
    }

    public class DeleteMatrixRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid Id { get; set; }
    }
}
