using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixSectionDetails.Responses
{
    public class MatrixSectionDetailResponse
    {
        public Guid Id { get; set; }
        public Guid MatrixSectionId { get; set; }
        public Guid ContentItemId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        [JsonIgnore]
        public DifficultyLevel Level { get; set; }
        public string LevelName => Level.ToString();
        [JsonIgnore]
        public QuestionType Type { get; set; }
        public string TypeName => Type.ToString();
        public int Quantity { get; set; }
    }
}
