using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models.Books
{
    public class UpdateBookRequest
    {
        [JsonRequired]
        public Guid BookId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 255 characters.")]
        public string Name { get; set; } = string.Empty;

        public Guid? SeriesId { get; set; }

        public string? Author { get; set; }

        public int? PublicationYear { get; set; }

        public string UpdatedBy { get; set; } = string.Empty;
    }
}
