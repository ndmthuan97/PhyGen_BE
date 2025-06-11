using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateContentItemRequest
    {
        [JsonRequired]
        public int ContentFlowId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string LearningOutcome { get; set; } = string.Empty;
    }

    public class UpdateContentItemRequest
    {
        [JsonRequired]
        public Guid Id { get; set; }
        [JsonRequired]
        public int ContentFlowId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string LearningOutcome { get; set; } = string.Empty;
    }
}
