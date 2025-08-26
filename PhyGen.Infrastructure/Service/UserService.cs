using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.Application.Authentication.Interface;
using PhyGen.Application.Authentication.Models.Requests;
using PhyGen.Application.Users.Dtos;
using PhyGen.Application.Users.Exceptions;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Specs;
using PhyGen.Infrastructure.Persistence.DbContexts;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure.Service;

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

        if (!string.IsNullOrEmpty(request.Phone))
        {
            var phoneRegex = new Regex(@"^0\d{9}$", RegexOptions.Compiled, TimeSpan.FromSeconds(1));
            // Kiểm tra số điện thoại phải 10 chữ số và bắt đầu bằng số 0
            if (!phoneRegex.IsMatch(request.Phone))
            {
                throw new ArgumentException("Số điện thoại phải có 10 số và bắt đầu từ số 0");
            }
        }

        // ---- Kiểm tra ngày sinh ----
        if (request.DateOfBirth.HasValue)
        {
            // Lấy giá trị DateTime thực (non-nullable)
            var dob = request.DateOfBirth.Value;

            // Tính số ngày tuổi
            var days = (DateTime.UtcNow.Date - dob.Date).TotalDays;
            if (days < 18 * 365.25)
            {
                throw new ArgumentException("Người dùng phải >= 18 tuổi");
            }

            // Chuẩn hóa về UTC
            dob = DateTime.SpecifyKind(dob, DateTimeKind.Utc);

            // Tính tuổi chính xác theo năm
            int age = DateTime.UtcNow.Year - dob.Year;
            if (dob.Date > DateTime.UtcNow.AddYears(-age).Date)
            {
                age--;
            }

            if (age < 18)
            {
                throw new ArgumentException("Bạn phải lớn hơn 18 tuổi");
            }

            // Gán lại giá trị chuẩn hóa
            request.DateOfBirth = dob;
        }

        // Cập nhật thông tin
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Phone = request.Phone;
        user.photoURL = request.PhotoURL;
        user.Gender = request.Gender;
        user.DateOfBirth = request.DateOfBirth;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return _mapper.Map<UserDtos>(user);
    }
    public async Task<Pagination<UserDtos>> GetAllProfilesAsync(ProfileFilter filter)
    {
        var query = _context.Users.AsQueryable();

        if (filter.Id.HasValue)
            query = query.Where(u => u.Id == filter.Id);

        if (!string.IsNullOrEmpty(filter.NameOrEmail)) 
        {
            var keyword = filter.NameOrEmail.Trim().ToLower(); 
            query = query.Where(u => u.Email.ToLower().Contains(keyword) 
                                || (u.FirstName + " " + u.LastName).ToLower().Contains(keyword) 
                                || u.FirstName.ToLower().Contains(keyword) 
                                || u.LastName.ToLower().Contains(keyword)); 
        }

        if (!string.IsNullOrEmpty(filter.Role))
            query = query.Where(u => u.Role == filter.Role);

        if (filter.IsConfirm.HasValue)
            query = query.Where(u => u.isConfirm == filter.IsConfirm.Value);

        if (filter.IsActive.HasValue)
            query = query.Where(u => u.IsActive == filter.IsActive.Value);

        if (filter.FromDate.HasValue)
            query = query.Where(u => u.CreatedAt >= DateTime.SpecifyKind(filter.FromDate.Value, DateTimeKind.Utc));

        if (filter.ToDate.HasValue)
            query = query.Where(u => u.CreatedAt <= DateTime.SpecifyKind(filter.ToDate.Value, DateTimeKind.Utc));

        query = query.OrderByDescending(u => u.CreatedAt);

        var totalCount = await query.CountAsync();

        var users = await query
            .Skip((filter.PageIndex - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        var userDtos = _mapper.Map<List<UserDtos>>(users);

        return new Pagination<UserDtos>(filter.PageIndex, filter.PageSize, totalCount, userDtos);
    }

    public async Task<object> LockUserAsync(Guid userId, LockAndUnlockUserRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        user.IsActive = false;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return new
        {
            Id = user.Id,
            Email = user.Email,
            IsActive = user.IsActive
        };
    }

    public async Task<object> UnLockUserAsync(Guid userId, LockAndUnlockUserRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        user.IsActive = true;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return new
        {
            Id = user.Id,
            Email = user.Email,
            IsActive = user.IsActive
        };
    }
}

