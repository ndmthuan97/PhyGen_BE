using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using MediatR;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PhyGen.Application.Authentication.Models.Requests;

namespace PhyGen.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseController<UserController>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper, IMediator mediator, ILogger<UserController> logger)
            : base(mediator, logger)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> ViewProfile()
        {
            // Lấy email từ claims trong JWT
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { message = "Email not found in token" });

            var profile = await _userService.ViewProfileAsync(email);

            if (profile == null)
                return NotFound(new { message = "User not found" });

            return Ok(profile);
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> EditProfile([FromBody] EditProfileRequest request)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { message = "Email not found in token" });

            var updatedUser = await _userService.EditProfileAsync(email, request);
            return Ok(updatedUser);
        }


    }
}
