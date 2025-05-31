using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using PhyGen.API.Controllers;
using PhyGen.Insfrastructure.Service;
using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.Application.Authentication.Interface;
using PhyGen.Shared.Constants;
using PhyGen.Application.Systems.Users;
using MediatR;
using PhyGen.Application.Authentication.Models.Requests;


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
            return Ok(response);
        }

        [HttpPost("confirmregisteration")]
        public async Task<IActionResult> Confirmregisteration(Confirmpassword _data)
        {
            var data = await _authService.ConfirmRegister(_data.userid, _data.email, _data.otptext);
            return Ok(data);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var dto = _mapper.Map<LoginDto>(request);
            var response = await _authService.LoginAsync(dto);
            return Ok(response);
        }

        [HttpGet("forgetpassword")]
        public async Task<IActionResult> Forgetpassword(string email)
        {
            var data = await _authService.ForgetPassword(email);
            return Ok(data);
        }

        [HttpPost("updatepassword")]
        public async Task<IActionResult> Updatepassword(Updatepassword _data)
        {
            var data = await this._authService.UpdatePassword(_data.email, _data.new_password, _data.otptext);
            return Ok(data);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var data = await _authService.ChangePasswordAsync(dto);
            return Ok(data);
        }

    }
}