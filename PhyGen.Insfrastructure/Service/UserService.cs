using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.Application.Authentication.Interface;
using PhyGen.Application.Authentication.Models.Requests;
using PhyGen.Application.Users.Exceptions;
using PhyGen.Domain.Entities;
using PhyGen.Insfrastructure.Persistence.DbContexts;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PhyGen.Insfrastructure.Service;

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
            throw new UserNotFoundException();
        }

        // Kiểm tra số điện thoại phải 10 chữ số và bắt đầu bằng số 0
        var phoneRegex = new Regex(@"^0\d{9}$",
            RegexOptions.Compiled,
            TimeSpan.FromSeconds(1));
        if (!phoneRegex.IsMatch(request.Phone))
        {
            throw new ArgumentException("Số điện thoại phải có 10 số và bắt đầu từ số 0");
        }

        // Kiểm tra DateOfBirth phải >= 18 tuổi
        if (!DateTime.TryParse(request.DayOfBirth, out DateTime dateOfBirth))
        {
            throw new ArgumentException("Vui lòng nhập chính xác ngày tháng năm sinh");
        }

        // Chuyển DateTime sang UTC nếu cần (nếu client gửi dạng local)
        dateOfBirth = DateTime.SpecifyKind(dateOfBirth, DateTimeKind.Utc);

        // Kiểm tra tuổi
        int age = DateTime.UtcNow.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > DateTime.UtcNow.AddYears(-age)) age--;

        if (age < 18)
        {
            throw new ArgumentException("Bạn phải lớn hơn 18 tuổi");
        }

        // Cập nhật thông tin
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Phone = request.Phone;
        user.photoURL = request.PhotoURL;
        user.Gender = request.Gender;
        user.DateOfBirth = dateOfBirth;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return _mapper.Map<UserDtos>(user);
    }
}
