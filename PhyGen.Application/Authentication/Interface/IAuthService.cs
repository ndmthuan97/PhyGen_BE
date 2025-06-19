using PhyGen.Application.Authentication.DTOs;
using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.Application.Authentication.DTOs.Responses;
using PhyGen.Application.Authentication.Responses;
using PhyGen.Shared;
using System.Threading.Tasks;

namespace PhyGen.Application.Authentication.Interface
{
    public interface IAuthService
    {
        Task<AuthenticationResponse> RegisterAsync(RegisterDto dto);
        Task<object> LoginAsync(LoginDto dto, string token);
        Task<AuthenticationResponse> ChangePasswordAsync(ChangePasswordDto dto);
        Task<AuthenticationResponse> ConfirmRegister(string username, string otptext);
        Task<object> ConfirmLogin(string username, string otptext);
        Task<AuthenticationResponse> ForgetPassword(string username);
        Task<AuthenticationResponse> UpdatePassword(string username, string Password, string Otptext);
    }
}
