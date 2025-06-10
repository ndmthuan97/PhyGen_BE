namespace PhyGen.API.Models
{
    public class CreateExamCategoryRequest
    {
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateExamCategoryRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
