using PhyGen.Application.Authentication.DTOs;
using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.Application.Authentication.Responses;
using PhyGen.Domain.Entities;
using System.Threading.Tasks;

namespace PhyGen.Application.Authentication.Interface
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
