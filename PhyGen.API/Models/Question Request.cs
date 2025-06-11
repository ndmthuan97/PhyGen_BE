using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateQuestionRequest
    {
        [Required]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Content must be between 1 and 255 characters.")]
        public string Content { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string? Image { get; set; }

        [JsonRequired]
        public Guid ChapterUnitId { get; set; }

        public string? Answer1 { get; set; }
        public string? Answer2 { get; set; }
        public string? Answer3 { get; set; }
        public string? Answer4 { get; set; }
        public string? Answer5 { get; set; }
        public string? Answer6 { get; set; }
        public string? CorrectAnswer { get; set; }
    }

    public class UpdateQuestionRequest
    {
        [JsonRequired]
        public Guid QuestionId { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Content must be between 1 and 255 characters.")]
        public string Content { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string? Image { get; set; }

        [JsonRequired]
        public Guid ChapterUnitId { get; set; }

        public string? Answer1 { get; set; }
        public string? Answer2 { get; set; }
        public string? Answer3 { get; set; }
        public string? Answer4 { get; set; }
        public string? Answer5 { get; set; }
        public string? Answer6 { get; set; }
        public string? CorrectAnswer { get; set; }
    }

    public class DeleteQuestionRequest
    {
        [Required(ErrorMessage = "QuestionId is required.")]
        public Guid QuestionId { get; set; }

        public string DeletedBy { get; set; } = string.Empty;
    }
}
