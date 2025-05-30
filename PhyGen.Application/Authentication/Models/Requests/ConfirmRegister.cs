using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Authentication.Models.Requests
{
    public class Confirmpassword
    {
        public int userid { get; set; }
        public string email { get; set; }
        public string otptext { get; set; }
    }
}
