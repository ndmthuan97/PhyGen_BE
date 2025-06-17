using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateSubjectBookRequest
    {
        [JsonRequired]
        public Guid SubjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Grade { get; set; }
    }
    public class UpdateSubjectBookRequest
    {
        [JsonRequired]
        public Guid Id { get; set; }
        [JsonRequired]
        public Guid SubjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Grade { get; set; }
    }
}
