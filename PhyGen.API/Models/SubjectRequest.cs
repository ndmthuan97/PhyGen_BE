using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateSubjectRequest
    {
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateSubjectRequest
    {
        [JsonRequired]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public string Name { get; set; } = string.Empty;
    }
}
