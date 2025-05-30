namespace PhyGen.API.Models.Questions
{
    public class UpdateQuestionRequest
    {
        public Guid Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public Guid UpdatedBy { get; set; }

        //public string Answer { get; set; } = string.Empty;
    }
}
