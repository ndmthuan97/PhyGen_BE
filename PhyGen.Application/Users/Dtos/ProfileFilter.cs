using PhyGen.Domain.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Users.Dtos
{
    public class ProfileFilter : BaseSpecParam
    {
        public string? NameOrEmail { get; set; }
        public bool? IsConfirm { get; set; }
        public bool? IsActive { get; set; }
        public string Role { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
