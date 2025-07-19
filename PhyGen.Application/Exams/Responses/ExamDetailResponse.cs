using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Exams.Responses
{
    public class ExamDetailResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Grade { get; set; }
        public int Year { get; set; }
        public string ImgUrl { get; set; } = string.Empty;

        public List<SectionDetailResponse> Sections { get; set; } = new();
    }

    public class SectionDetailResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string SectionType { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }

        public List<QuestionInSectionResponse> Questions { get; set; } = new();
    }

    public class QuestionInSectionResponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? Answer1 { get; set; }
        public string? Answer2 { get; set; }
        public string? Answer3 { get; set; }
        public string? Answer4 { get; set; }
        public string? CorrectAnswer { get; set; }
        public double? Score { get; set; } 
    }

}
