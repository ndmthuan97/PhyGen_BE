using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateContentItemRequest
    {
        public string Title { get; set; } = string.Empty;
        public string LearningOutcome { get; set; } = string.Empty;
    }

    public class UpdateContentItemRequest
    {
        [JsonRequired]
        public Guid ContentItemId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string LearningOutcome { get; set; } = string.Empty;
    }
}
