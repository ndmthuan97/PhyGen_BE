using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.Application.Authentication.Models.Requests;
using PhyGen.Domain.Entities;
using PhyGen.Insfrastructure.Persistence.DbContexts;
using System.Security.Claims;
using System.Threading.Tasks;


public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public UserService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDtos?> ViewProfileAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        if (user == null)
            return null;

        var userProfileDto = _mapper.Map<UserDtos>(user);
        return userProfileDto;
    }
    public async Task<UserDtos> EditProfileAsync(string email, EditProfileRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        if (user == null)
        {
            throw new Exception("User not found");
        }
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Address = request.Address;
        user.Phone = request.Phone;
        user.photoURL = request.PhotoURL;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return _mapper.Map<UserDtos>(user);
    }

}
