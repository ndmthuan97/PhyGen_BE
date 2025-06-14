using System.Text.Json.Serialization;

namespace PhyGen.API.Models
{
    public class CreateMatrixContentItemRequest
    {
        [JsonRequired]
        public Guid MatrixId { get; set; }

        [JsonRequired]
        public Guid ContentItemId { get; set; }
    }

    public class UpdateMatrixContentItemRequest
    {
        [JsonRequired]
        public int MatrixContentItemId { get; set; }

        [JsonRequired]
        public Guid MatrixId { get; set; }

        [JsonRequired]
        public Guid ContentItemId { get; set; }
    }
}
