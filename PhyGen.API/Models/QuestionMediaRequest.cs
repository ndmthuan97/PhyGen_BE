using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateQuestionMediaRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid QuestionId { get; set; }

        public string MediaType { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }

    public class UpdateQuestionMediaRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid Id { get; set; }

        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid QuestionId { get; set; }

        public string MediaType { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }

    public class DeleteQuestionMediaRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid Id { get; set; }
    }
}
