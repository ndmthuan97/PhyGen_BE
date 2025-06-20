using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateTopicRequest
    {
        [JsonRequired]
        public Guid ChapterId { get; set; }
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public string Name { get; set; } = string.Empty;
        public int OrderNo { get; set; }
    }
    public class UpdateTopicRequest
    {
        [JsonRequired]
        public Guid Id { get; set; }
        [JsonRequired]
        public Guid ChapterId { get; set; }
        [Required(ErrorMessage = "Trường này không được để trống.")]
        public string Name { get; set; } = string.Empty;
        public int OrderNo { get; set; }
    }
    public class DeleteTopicRequest
    {
        [JsonRequired]
        public Guid Id { get; set; }
    }
}