using PhyGen.Application.Authentication.DTOs;
using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.Application.Authentication.Responses;
using System.Threading.Tasks;

namespace PhyGen.Application.Authentication.Interface
{
    public interface IAuthService
    {
        Task<AuthenticationResponse> RegisterAsync(RegisterDto dto);
        Task<AuthenticationResponse> LoginAsync(LoginDto dto);
    }
}
