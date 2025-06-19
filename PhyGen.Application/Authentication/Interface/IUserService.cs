using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.Application.Authentication.Models.Requests;
using PhyGen.Application.Users.Dtos;
using PhyGen.Domain.Specs;
using System.Threading.Tasks;

namespace PhyGen.Application.Authentication.Interface
{
    public interface IUserService
    {
        Task<UserDtos?> ViewProfileAsync(string email);
        Task<UserDtos> EditProfileAsync(string email, EditProfileRequest request);
        Task<Pagination<UserDtos>> GetAllProfilesAsync(ProfileFilter filter);
        Task<object> LockUserAsync(Guid userId, LockAndUnlockUserRequest request);
        Task<object> UnLockUserAsync(Guid userId, LockAndUnlockUserRequest request);
    }
}

