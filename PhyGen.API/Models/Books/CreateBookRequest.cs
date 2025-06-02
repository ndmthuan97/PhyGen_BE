using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models.Books
{
    public class CreateBookRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 255 characters.")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("seriesId")]
        public string? SeriesIdRaw { get; set; }

        [JsonIgnore]
        public Guid? SeriesId
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SeriesIdRaw))
                    return null;

                if (Guid.TryParse(SeriesIdRaw, out var guid))
                    return guid;

                return null;
            }
        }

        public string? Author { get; set; }

        public int? PublicationYear { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
    }
}
