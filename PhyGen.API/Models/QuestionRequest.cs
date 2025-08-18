using PhyGen.Shared.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateQuestionRequest
    {
        [JsonPropertyName("topicId")]
        public string? TopicIdRaw { get; set; }

        [JsonIgnore]
        public Guid? TopicId
        {
            get
            {
                if (string.IsNullOrWhiteSpace(TopicIdRaw))
                    return null;

                if (Guid.TryParse(TopicIdRaw, out var parsed))
                    return parsed;

                throw new ValidationException("TopicId không hợp lệ.");
            }
        }

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

        public StatusQEM Status { get; set; }
    }

    public class UpdateQuestionRequest
    {
        [JsonRequired]
        public Guid Id { get; set; }
        [JsonPropertyName("topicId")]
        public string? TopicIdRaw { get; set; }

        [JsonIgnore]
        public Guid? TopicId
        {
            get
            {
                if (string.IsNullOrWhiteSpace(TopicIdRaw))
                    return null;

                if (Guid.TryParse(TopicIdRaw, out var parsed))
                    return parsed;

                throw new ValidationException("TopicId không hợp lệ.");
            }
        }

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

        public StatusQEM Status { get; set; }
    }

    public class DeleteQuestionRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid Id { get; set; }
    }

    public class UpdateQuestionStatusRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public List<Guid> Ids { get; set; }

        public StatusQEM Status { get; set; }
    }
}
