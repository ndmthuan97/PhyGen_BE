using PhyGen.Shared.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateQuestionRequest
    {
        public Guid TopicId { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Nội dung câu hỏi không được vượt quá 1000 ký tự.")]
        public string Content { get; set; } = string.Empty;
        
        public DifficultyLevel Level { get; set; }
        
        public QuestionType Type { get; set; }
        
        public string? Answer1 { get; set; }
        public string? Answer2 { get; set; }
        public string? Answer3 { get; set; }
        public string? Answer4 { get; set; }
        public string? Answer5 { get; set; }
        public string? Answer6 { get; set; }

        [Required(ErrorMessage = "Trường này không được để trống.")]
        [Range(10, 12, ErrorMessage = "Lớp học phải nằm trong khoảng 10 đến 12.")]
        public int Grade { get; set; }
    }

    public class UpdateQuestionRequest
    {
        public Guid Id { get; set; }

        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid TopicId { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Nội dung câu hỏi không được vượt quá 1000 ký tự.")]
        public string Content { get; set; } = string.Empty;

        public DifficultyLevel Level { get; set; }

        public QuestionType Type { get; set; }

        public string? Answer1 { get; set; }
        public string? Answer2 { get; set; }
        public string? Answer3 { get; set; }
        public string? Answer4 { get; set; }
        public string? Answer5 { get; set; }
        public string? Answer6 { get; set; }

        [Required(ErrorMessage = "Trường này không được để trống.")]
        [Range(10, 12, ErrorMessage = "Lớp học phải nằm trong khoảng 10 đến 12.")]
        public int Grade { get; set; }
    }

    public class DeleteQuestionRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid Id { get; set; }
    }
}
