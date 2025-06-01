using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models.Chapters
{
    public class UpdateChapterRequest
    {
        [JsonRequired]
        public Guid ChapterId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 255 characters.")]
        public string Name { get; set; } = string.Empty;

        public Guid? CurriculumId { get; set; }

        public Guid? BookId { get; set; }

        public int OrderNo { get; set; }

        public string UpdatedBy { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
