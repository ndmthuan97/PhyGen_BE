using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using PhyGen.API.Controllers;
using PhyGen.Insfrastructure.Service;
using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.Application.Authentication.Interface;
using PhyGen.Shared.Constants;
using MediatR;
using PhyGen.Application.Authentication.Models.Requests;
using PhyGen.Application.Authentication.Responses;
using PhyGen.Application.Authentication.DTOs.Responses;


namespace PhyGen.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController<AuthController>
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IMapper mapper, IMediator mediator, ILogger<AuthController> logger)
            : base(mediator, logger)
        {
            _authService = authService;
            _mapper = mapper;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var dto = _mapper.Map<RegisterDto>(request);
            var response = await _authService.RegisterAsync(dto);

            return response.StatusCode switch
            {
                Shared.Constants.StatusCode.EmailAlreadyExists => Conflict(response),          
                Shared.Constants.StatusCode.InvalidPasswordFormat => UnprocessableEntity(response),  
                Shared.Constants.StatusCode.PasswordMismatch => UnprocessableEntity(response),      
                Shared.Constants.StatusCode.RegisterSuccess => Ok(response),                  
                _ => BadRequest(response)                                                     
            };
        }

        [HttpPost("confirmregistration")]
        public async Task<IActionResult> ConfirmRegistration([FromBody] Confirmpassword data)
        {
            var response = await _authService.ConfirmRegister(data.email, data.otptext);

            return response.StatusCode switch
            {
                Shared.Constants.StatusCode.EmailAlreadyExists => Conflict(response),              
                Shared.Constants.StatusCode.UserAuthenticationFailed => Unauthorized(response),   
                Shared.Constants.StatusCode.AccountNotConfirmed => Conflict(response),          
                Shared.Constants.StatusCode.RegisterSuccess => Ok(response),                   
                _ => BadRequest(response)                                                       
            };
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        [FromHeader(Name = "Authorization")] string? authorizationHeader)
        {
            var dto = _mapper.Map<LoginDto>(request);
            string token = null;

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                token = authorizationHeader.Substring("Bearer ".Length).Trim();
            }

            var result = await _authService.LoginAsync(dto, token);

            if (result is AuthenticationResponse authResponse)
            {
                // Thất bại => chỉ trả về email, mã lỗi và message
                return BadRequest(new
                {
                    authResponse.Email,
                    authResponse.StatusCode,
                    authResponse.Message
                });
            }

            if (result is LoginResponse loginResponse)
            {
                // Thành công => trả về cả Token và Role
                return Ok(new
                {
                    loginResponse.Response.Email,
                    loginResponse.Response.StatusCode,
                    loginResponse.Response.Message,
                    Token = loginResponse.Token,
                    Role = loginResponse.Role
                });
            }

            // Fallback
            return StatusCode(500, "Unexpected error occurred.");
        }

        [HttpPost("confirmlogin")]
        public async Task<IActionResult> Confirmlogin(Confirmpassword _data)
        {
            var response = await _authService.ConfirmLogin(_data.email, _data.otptext);
            if (response is AuthenticationResponse authResponse)
            {
                // Thất bại => chỉ trả về email, mã lỗi và message
                return BadRequest(new
                {
                    authResponse.Email,
                    authResponse.StatusCode,
                    authResponse.Message
                });
            }

            if (response is LoginResponse loginResponse)
            {
                // Thành công => trả về cả Token và Role
                return Ok(new
                {
                    loginResponse.Response.Email,
                    loginResponse.Response.StatusCode,
                    loginResponse.Response.Message,
                    Token = loginResponse.Token,
                    Role = loginResponse.Role
                });
            }
            // Fallback
            return StatusCode(500, "Unexpected error occurred.");
        }

        [HttpGet("forgetpassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            var data = await _authService.ForgetPassword(email);

            return data.StatusCode switch
            {
                Shared.Constants.StatusCode.InvalidUser => NotFound(data),
                Shared.Constants.StatusCode.MustLoginWithGoogle => BadRequest(data),
                Shared.Constants.StatusCode.OtpSendSuccess => Ok(data),
                _ => StatusCode(500, data)
            };
        }

        [HttpPost("updatepassword")]
        public async Task<IActionResult> UpdatePassword(Updatepassword _data)
        {
            var data = await _authService.UpdatePassword(_data.email, _data.new_password, _data.otptext);

            return data.StatusCode switch
            {
                Shared.Constants.StatusCode.InvalidUser => NotFound(data),
                Shared.Constants.StatusCode.InvalidOtp => BadRequest(data),
                Shared.Constants.StatusCode.InvalidPasswordFormat => BadRequest(data),
                Shared.Constants.StatusCode.ChangedPasswordSuccess => Ok(data),
                _ => StatusCode(500, data)
            };
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var data = await _authService.ChangePasswordAsync(dto);

            return data.StatusCode switch
            {
                Shared.Constants.StatusCode.EmailDoesNotExists => NotFound(data),
                Shared.Constants.StatusCode.InvalidPassword => Unauthorized(data),
                Shared.Constants.StatusCode.InvalidPasswordFormat => BadRequest(data),
                Shared.Constants.StatusCode.PasswordMismatch => BadRequest(data),
                Shared.Constants.StatusCode.ChangedPasswordSuccess => Ok(data),
                _ => StatusCode(500, data)
            };
        }

    }
}