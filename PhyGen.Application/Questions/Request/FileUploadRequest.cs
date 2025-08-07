using Microsoft.AspNetCore.Http;
using PhyGen.Domain.Specs.Topic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Questions.Request
{
    public class FileUploadRequest
    {
        [Required]
        public IFormFile File { get; set; }
        public int Grade { get; set; }
    }
}
