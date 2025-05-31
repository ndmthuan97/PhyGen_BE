using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models.Curriculums
{
    public class CreateCurriculumRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 255 characters.")]
        public string Name { get; set; } = string.Empty;

        public string? Grade { get; set; }

        public string? Description { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
    }
}
