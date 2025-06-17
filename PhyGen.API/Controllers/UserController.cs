using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using MediatR;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PhyGen.Application.Authentication.Models.Requests;
using PhyGen.Application.Authentication.Interface;
using PhyGen.Application.Users.Exceptions;
using PhyGen.Application.Users.Dtos;

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

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> ViewProfile()
        {
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

        [AllowAnonymous]
        [HttpGet("getAllProfiles")]
        public async Task<IActionResult> GetAllProfiles(
            [FromQuery] Guid? id,
            [FromQuery] bool? isConfirm,
            [FromQuery] bool? isActive,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            var filter = new ProfileFilter
            {
                Id = id,
                IsConfirm = isConfirm,
                IsActive = isActive,
                FromDate = fromDate,
                ToDate = toDate
            };

            var profiles = await _userService.GetAllProfilesAsync(filter);
            return Ok(profiles);
        }
    }
}
