using PhyGen.Application.Systems.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PhyGen.Application.Authentication.Interface
{
    public interface IUserService
    {
        Task<bool> Authencate(LoginRequest request);

        Task<bool> Register(RegisterRequest request);
    }
  
}
