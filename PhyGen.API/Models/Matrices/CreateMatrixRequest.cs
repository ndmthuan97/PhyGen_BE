using System.ComponentModel.DataAnnotations;

namespace PhyGen.API.Models.Matrices
{
    public class CreateMatrixRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 255 characters.")]
        public string Name { get; set; } = string.Empty;

        public Guid CreatedBy { get; set; }
    }
}
