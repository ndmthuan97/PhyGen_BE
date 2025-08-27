using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Exams.Models
{
    public class ExamExportModel
    {
        public string Title { get; set; } = "ĐỀ THI";
        public string Subject { get; set; } = "VẬT LÍ";
        public string EducationDepartment { get; set; } = "SỞ GD & ĐT";
        public string School { get; set; } = "TRƯỜNG THPT";
        public int Grade { get; set; }
        public int Year { get; set; }
        public string Duration { get; set; } = "45 phút";
        public string Code { get; set; } = "01";
        public List<SectionExportDto> Sections { get; set; } = new();
    }

    public class SectionExportDto
    {
        public string Title { get; set; } = string.Empty;
        public string SectionType { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public List<QuestionExportDto> Questions { get; set; } = new();
    }

    public class QuestionExportDto
    {
        public string Content { get; set; } = string.Empty;
        public string? Answer1 { get; set; }
        public string? Answer2 { get; set; }
        public string? Answer3 { get; set; }
        public string? Answer4 { get; set; }   
        public string? Answer5 { get; set; }       
        public string? Answer6 { get; set; }
        public double? Score { get; set; }
        public List<string>? ImageUrls { get; set; }
    }  
}
