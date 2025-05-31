using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models.Curriculums
{
    public class DeleteCurriculumRequest
    {
        [JsonRequired]
        public Guid CurriculumId { get; set; }

        public string DeletedBy { get; set; } = string.Empty;
    }
}
