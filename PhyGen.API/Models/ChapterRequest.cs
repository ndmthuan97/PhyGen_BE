using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateChapterRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 255 characters.")]
        public string Name { get; set; } = string.Empty;

        [JsonRequired]
        public Guid SubjectCurriculumId { get; set; }

        public int? OrderNo { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
    }

    public class UpdateChapterRequest
    {
        [JsonRequired]
        public Guid ChapterId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 255 characters.")]

        public string Name { get; set; } = string.Empty;

        [JsonRequired]
        public Guid SubjectCurriculumId { get; set; }

        public int? OrderNo { get; set; }

        public string UpdatedBy { get; set; } = string.Empty;
    }
}
