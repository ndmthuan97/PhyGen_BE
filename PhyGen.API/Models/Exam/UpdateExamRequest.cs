using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models.Exam
{
    public class UpdateExamRequest
    {
        [JsonRequired]
        public Guid ExamId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 255 characters.")]

        public string Title { get; set; } = string.Empty;

        public Guid UpdatedBy { get; set; }
    }
}
