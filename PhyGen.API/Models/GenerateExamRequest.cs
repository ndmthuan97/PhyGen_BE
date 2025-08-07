namespace PhyGen.API.Models
{
    public class GenerateExamRequest
    {
        public string ExamType { get; set; }
        public string Grade { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public object Matrix { get; set; }
    }
}
