using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateQuestionSectionRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid QuestionId { get; set; }

        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid SectionId { get; set; }

        public double? Score { get; set; }
    }

    public class UpdateQuestionSectionRequest
    {
        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid Id { get; set; }

        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid QuestionId { get; set; }

        [JsonRequired]
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public Guid SectionId { get; set; }

        public double? Score { get; set; }
    }
}
