using Microsoft.Extensions.Configuration;
using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.Application.Authentication.Interface;
using PhyGen.Application.Authentication.Responses;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Exceptions;
using PhyGen.Insfrastructure.Persistence.DbContexts;
using PhyGen.Shared.Constants;

namespace PhyGen.Insfrastructure.Service;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(AppDbContext context, IJwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthenticationResponse> RegisterAsync(RegisterDto dto)
    {
        if (_context.Users.Any(u => u.Email.ToLower() == dto.Email.ToLower()))
        {
            throw new AppException(StatusCode.EmailAlreadyExists);
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Phone = dto.PhoneNumber,
            Email = dto.Email,
            Role = "User",
            Password = dto.Password // ❗Lưu mật khẩu plaintext (KHÔNG NÊN dùng thật)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new AuthenticationResponse
        {
            Email = user.Email,
            Token = _jwtTokenGenerator.GenerateToken(user)
        };
    }

    public async Task<AuthenticationResponse> LoginAsync(LoginDto dto)
    {
        var email = dto.Email.Trim().ToLower();
        var user = _context.Users.FirstOrDefault(u => u.Email.ToLower() == email);
        if (user == null)
            throw new AuthException(StatusCode.UserNotFound);

        if (user.Password != dto.Password)
            throw new AuthException(StatusCode.InvalidPassword);

        return new AuthenticationResponse
        {
            Email = user.Email,
            Token = _jwtTokenGenerator.GenerateToken(user)
        };
    }
}
