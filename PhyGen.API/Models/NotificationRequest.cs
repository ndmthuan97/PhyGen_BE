using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateNotificationRequest
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 255 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Message is required.")]
        public string Message { get; set; } = string.Empty;

        [Required(ErrorMessage = "UserId is required.")]
        public Guid UserId { get; set; }
    }

    public class UpdateNotificationRequest
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 255 characters.")]
        public string Title { get; set; } = string.Empty;

        [JsonRequired]
        public int Id { get; set; }
        [JsonRequired]
        public string Message { get; set; }
    }

    public class DeleteNotificationRequest
    {
        [Required(ErrorMessage = "ExamId is required.")]
        public Guid Id { get; set; }

    }
}
