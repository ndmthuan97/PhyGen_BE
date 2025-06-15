using System.Threading.Tasks;
using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.Application.Authentication.Models.Requests;
using PhyGen.Application.Users.Dtos;

namespace PhyGen.Application.Authentication.Interface
{
    public interface IUserService
    {
        Task<UserDtos?> ViewProfileAsync(string email);
        Task<UserDtos> EditProfileAsync(string email, EditProfileRequest request);
        Task<List<UserDtos>> GetAllProfilesAsync(ProfileFilter filter);
    }
}

