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

        [JsonPropertyName("curriculumId")]
        public string? CurriculumIdRaw { get; set; }

        [JsonIgnore]
        public Guid? CurriculumId
        {
            get
            {
                if (string.IsNullOrWhiteSpace(CurriculumIdRaw))
                    return null;

                if (Guid.TryParse(CurriculumIdRaw, out var guid))
                    return guid;

                return null;
            }
        }

        public int OrderNo { get; set; }

        public string UpdatedBy { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
