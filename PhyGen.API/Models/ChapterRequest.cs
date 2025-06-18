using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateChapterRequest
    {
        [JsonRequired]
        public Guid SubjectBookId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int OrderNo { get; set; }
    }
    public class UpdateChapterRequest
    {
        [JsonRequired]
        public Guid Id { get; set; }
        [JsonRequired]
        public Guid SubjectBookId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int OrderNo { get; set; }
    }
    public class DeleteChapterRequest
    {
        [JsonRequired]
        public Guid Id { get; set; }
    }
}
