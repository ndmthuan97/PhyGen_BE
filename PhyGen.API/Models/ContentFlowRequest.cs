using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateContentFlowRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [JsonRequired]
        public Guid SubjectId { get; set; }
    }

    public class UpdateContentFlowRequest
    {
        [JsonRequired]
        public int ContentFlowId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [JsonRequired]
        public Guid SubjectId { get; set; }
    }
}
