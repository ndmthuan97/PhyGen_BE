using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models.Answers
{
    public class CreateAnswerRequest
    {
        [Required(ErrorMessage = "Content is required.")]

        public string Content { get; set; } = string.Empty;
        public bool IsCorrect { get; set; } = false;

        [JsonRequired]
        public string? CreatedBy { get; set; }
    }
}
