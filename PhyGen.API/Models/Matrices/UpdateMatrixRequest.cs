using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhyGen.API.Models.Matrices
{
    public class UpdateMatrixRequest
    {
        [JsonRequired]
        public Guid MatrixId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 255 characters.")]

        public string Name { get; set; } = string.Empty;

        public Guid CreatedBy { get; set; }
    }
}
