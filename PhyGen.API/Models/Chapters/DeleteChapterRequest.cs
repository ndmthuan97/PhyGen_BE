using System.Text.Json.Serialization;

namespace PhyGen.API.Models.Chapters
{
    public class DeleteChapterRequest
    {
        [JsonRequired]
        public Guid ChapterId { get; set; }
        public string DeletedBy { get; set; } = string.Empty;
    }
}
