namespace PhyGen.API.Models
{
    public class GenerateExamRequest
    {
        public string Title { get; set; }
        public string ExamType { get; set; }
        public string Grade { get; set; }
        public int Year { get; set; }
        public IFormFile MatrixFile { get; set; }
    }
}
