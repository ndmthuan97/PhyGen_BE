using System.Text.Json.Serialization;

namespace PhyGen.API.Models.ChapterUnits
{
    public class UpdateChapterUnitRequest
    {
        [JsonRequired]
        public Guid ChapterUnitId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int? OrderNo { get; set; }
        [JsonRequired]
        public Guid ChapterId { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
    }
}
