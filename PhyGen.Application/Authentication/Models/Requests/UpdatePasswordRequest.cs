using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Authentication.Models.Requests
{
    public class Updatepassword
    {
        public string email { get; set; }
        public string new_password { get; set; }
        public string otptext { get; set; }
    }
}
