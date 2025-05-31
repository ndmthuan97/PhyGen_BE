using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models.Answers
{
    public class UpdateAnswerRequest
    {
        [JsonRequired]
        public Guid CurriculumId { get; set; }

        [Required(ErrorMessage = "Name is required.")]

        public string Content { get; set; } = string.Empty;
        public bool IsCorrect { get; set; } = false;

        [JsonRequired]
        public Guid UpdatedBy { get; set; }
    }
}
