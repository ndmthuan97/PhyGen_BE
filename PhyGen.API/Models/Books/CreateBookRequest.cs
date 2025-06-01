using System.ComponentModel.DataAnnotations;

namespace PhyGen.API.Models.Books
{
    public class CreateBookRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 255 characters.")]
        public string Name { get; set; } = string.Empty;

        public Guid? SeriesId { get; set; }

        public string? Author { get; set; }

        public int? PublicationYear { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
    }
}
