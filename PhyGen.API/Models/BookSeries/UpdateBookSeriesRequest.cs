using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models.BookSeries
{
    public class UpdateBookSeriesRequest
    {
        [JsonRequired]
        public Guid BookSeriesId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 255 characters.")]
        public string Name { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
    }
}
