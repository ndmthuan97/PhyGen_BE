using PhyGen.Shared.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateQuestionRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid TopicId { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Nội dung câu hỏi không được vượt quá 1000 ký tự.")]
        public string Content { get; set; } = string.Empty;
        
        public DifficultyLevel Level { get; set; }
        
        public QuestionType Type { get; set; }
        
        public string? Image { get; set; }
    }

    public class UpdateQuestionRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid Id { get; set; }

        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid TopicId { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Nội dung câu hỏi không được vượt quá 1000 ký tự.")]
        public string Content { get; set; } = string.Empty;

        public DifficultyLevel Level { get; set; }

        public QuestionType Type { get; set; }

        public string? Image { get; set; }
    }

    public class DeleteQuestionRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid Id { get; set; }
    }
}
