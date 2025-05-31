using PhyGen.Application.Authentication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Authentication.DTOs.Responses
{
    public class LoginResponse
    {
        public AuthenticationResponse Response { get; set; }
        public string Token { get; set; }
    }
}
