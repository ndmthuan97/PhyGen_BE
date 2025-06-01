using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using MediatR;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PhyGen.Application.Authentication.Models.Requests;
using PhyGen.Application.Authentication.Interface;
using PhyGen.Application.Exceptions.Users;

namespace PhyGen.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseController<UserController>
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService, IMediator mediator, ILogger<UserController> logger)
            : base(mediator, logger)
        {
            _userService = userService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> ViewProfile()
        {
            // Lấy email từ claims trong JWT
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
                return Unauthorized(new UserNotFoundException());

            var profile = await _userService.ViewProfileAsync(email);

            if (profile == null)
                return NotFound(new UserNotFoundException());

            return Ok(profile);
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> EditProfile([FromBody] EditProfileRequest request)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized(new UserNotFoundException());

            var updatedUser = await _userService.EditProfileAsync(email, request);
            return Ok(updatedUser);
        }


    }
}
