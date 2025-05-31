using System.Text.Json.Serialization;

namespace PhyGen.API.Models.ChapterUnits
{
    public class CreateChapterUnitRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? OrderNo { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        [JsonRequired]
        public Guid ChapterId { get; set; }
    }
}
