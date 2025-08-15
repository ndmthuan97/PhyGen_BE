using PhyGen.Shared.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateExamRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid ExamCategoryId { get; set; }

        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid UserId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }
        [Required(ErrorMessage = "Trường này không được để trống.")]
        [Range(10, 12, ErrorMessage = "Lớp học phải nằm trong khoảng 10 đến 12.")]
        public int Grade { get; set; } = 0;
        public int Year { get; set; } = 0;
        public int? TotalQuestionCount { get; set; } = null;
        public int VersionCount { get; set; } = 1;
        public bool RandomizeQuestions { get; set; } = false;
        public string ImgUrl { get; set; } = string.Empty;
    }

    public class UpdateExamRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid Id { get; set; }

        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid ExamCategoryId { get; set; }

        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid UserId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }
        [Required(ErrorMessage = "Trường này không được để trống.")]
        [Range(10, 12, ErrorMessage = "Lớp học phải nằm trong khoảng 10 đến 12.")]
        public int Grade { get; set; }
        public int Year { get; set; }
        public int? TotalQuestionCount { get; set; } = null;
        public int VersionCount { get; set; } = 1;
        public bool RandomizeQuestions { get; set; } = false;
        public string ImgUrl { get; set; } = string.Empty;
        public StatusQEM Status { get; set; }
    }

    public class DeleteExamRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid Id { get; set; }
    }
}
