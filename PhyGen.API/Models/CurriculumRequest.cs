using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateCurriculumRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 255 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Grade is required.")]
        public int Grade { get; set; }
    }

    public class UpdateCurriculumRequest
    {
        [JsonRequired]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 255 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Grade is required.")]
        public int Grade { get; set; }
    }

    public class DeleteCurriculumRequest
    {
        [JsonRequired]
        public Guid Id { get; set; }
    }
}
