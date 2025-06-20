using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateSubjectBookRequest
    {
        [JsonRequired]
        public Guid SubjectId { get; set; }
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public int Grade { get; set; }
    }
    public class UpdateSubjectBookRequest
    {
        [JsonRequired]
        public Guid Id { get; set; }
        [JsonRequired]
        public Guid SubjectId { get; set; }
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public int Grade { get; set; }
    }
    public class DeleteSubjectBookRequest
    {
        [JsonRequired]
        public Guid Id { get; set; }
    }
}
