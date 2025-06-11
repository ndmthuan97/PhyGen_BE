using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateSubjectRequest
    {
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateSubjectRequest
    {
        [JsonRequired]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
