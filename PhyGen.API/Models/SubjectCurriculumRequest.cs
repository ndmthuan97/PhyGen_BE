using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateSubjectCurriculumRequest
    {
        [JsonRequired]
        public Guid SubjectId { get; set; }
        [JsonRequired]
        public Guid CurriculumId { get; set; }
    }

    public class UpdateSubjectCurriculumRequest
    {
        [JsonRequired]
        public Guid Id { get; set; }
        [JsonRequired]
        public Guid SubjectId { get; set; }
        [JsonRequired]
        public Guid CurriculumId { get; set; }
    }
}
