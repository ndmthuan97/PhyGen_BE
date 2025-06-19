using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.Application.Authentication.Interface;
using PhyGen.Application.Authentication.Models.Requests;
using PhyGen.Application.Users.Dtos;
using PhyGen.Application.Users.Exceptions;
using PhyGen.Domain.Specs;
using PhyGen.Shared;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PhyGen.API.Controllers
{
    [ApiController]
    [Route("api/users")]
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
        [ProducesResponseType(typeof(ApiResponse<Pagination<UserDtos>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllProfiles(
        [FromQuery] Guid? id,
        [FromQuery] string? Role,
        [FromQuery] bool? isConfirm,
        [FromQuery] bool? isActive,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] int pageIndex = 1)
        {
            var filter = new ProfileFilter
            {
                Id = id,
                Role = Role,
                IsConfirm = isConfirm,
                IsActive = isActive,
                FromDate = fromDate,
                ToDate = toDate,
                PageIndex = pageIndex,
                PageSize = 10
            };

            var profiles = await _userService.GetAllProfilesAsync(filter);
            return Ok(profiles);
        }



        [HttpPut("lock")]
        public async Task<IActionResult> LockUser([FromQuery] Guid UserId)
        {
            var request = new LockAndUnlockUserRequest { UserId = UserId };
            var updatedUser = await _userService.LockUserAsync(UserId, request);
            return Ok(updatedUser);
        }

        [HttpPut("unlock")]
        public async Task<IActionResult> UnLockUser([FromQuery] Guid UserId)
        {
            var request = new LockAndUnlockUserRequest { UserId = UserId };
            var updatedUser = await _userService.UnLockUserAsync(UserId, request);
            return Ok(updatedUser);
        }
    }
}
