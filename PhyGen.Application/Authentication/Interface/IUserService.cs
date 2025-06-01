using System.Threading.Tasks;
using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.Application.Authentication.Models.Requests;

public interface IUserService
{
    Task<UserDtos?> ViewProfileAsync(string email);
    Task<UserDtos> EditProfileAsync(string email, EditProfileRequest request);

}
