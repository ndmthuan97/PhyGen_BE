namespace PhyGen.API.Models.Questions
{
    public class CreateQuestionRequest
    {
        public string Content { get; set; } = string.Empty;

        public Guid CreatedBy { get; set; }

        //public string Answer { get; set; } = string.Empty;
    }
}
