using AutoMapper;
using AutoMapper.Internal;
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
    private readonly IEmailService _emailService;

    public UserService(AppDbContext context, IEmailService emailService, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        _emailService = emailService;  
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
        await SendMail(user.Email, "lock");
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
        await SendMail(user.Email, "unlock");
        await _context.SaveChangesAsync();

        return new
        {
            Id = user.Id,
            Email = user.Email,
            IsActive = user.IsActive
        };
    }
    private async Task SendMail(string email, string type)
    {
        var mailrequest = new EmailRequest();
        mailrequest.Email = email;

        if (type == "lock")
        {
            mailrequest.Subject = "Thông báo khóa tài khoản";
            mailrequest.Emailbody = GenerateAccountLockedEmail(email);
        }
        else if (type == "unlock")
        {
            mailrequest.Subject = "Mở khóa tài khoản";
            mailrequest.Emailbody = GenerateAccountUnlockedEmail(email);
        }

        await _emailService.SendEmailAsync(mailrequest);
    }

    private static string GenerateAccountLockedEmail(string name)
    {
        string supportEmail = "phygenfptuni@gmail.com";
        string emailBody = "<div style='width: 100%; background-color: #f4f4f4; padding: 20px 0; font-family: Arial, sans-serif;'>";
        emailBody += "<div style='max-width: 600px; margin: auto; background: #fff; padding: 30px; border-radius: 10px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);'>";
        emailBody += "<h2 style='color: #333; margin-top: 0;'>Tài khoản của bạn đã bị khóa</h2>";
        emailBody += "<p style='font-size: 16px; color: #555;'>Xin chào " + name + ",</p>";
        emailBody += "<p style='font-size: 16px; color: #555;'>Tài khoản của bạn trên <strong>Hệ thống PhyGen</strong> tạm thời bị khóa để đảm bảo an toàn.</p>";
        emailBody += "<p style='font-size: 15px; color: #555;'><strong>Lý do:</strong> " + "Vi phạm quy tắc của hệ thống" + "</p>";      
        emailBody += "<div style='margin: 24px 0; padding: 16px; background:#fff6f6; border:1px solid #f5c2c7; border-radius:8px;'>";
        emailBody += "<p style='margin:0; font-size:14px; color:#a94442;'>Trong thời gian bị khóa, bạn sẽ không thể đăng nhập hoặc thực hiện các thao tác liên quan đến tài khoản.</p>";
        emailBody += "</div>";
        emailBody += "<p style='font-size: 14px; color: #888;'>Nếu bạn cần hỗ trợ, vui lòng liên hệ <a href='mailto:" + supportEmail + "'>" + supportEmail + "</a>.</p>";
        emailBody += "<hr style='margin-top: 40px; border: none; border-top: 1px solid #eee;' />";
        emailBody += "<p style='font-size: 12px; color: #aaa;'>Trân trọng,<br/>Đội ngũ PhyGen</p>";
        emailBody += "</div>";
        emailBody += "</div>";
        return emailBody;
    }

    private static string GenerateAccountUnlockedEmail(string name)
    {
        string email = "https://phygen.io.vn/login";
        string emailBody = "<div style='width: 100%; background-color: #f4f4f4; padding: 20px 0; font-family: Arial, sans-serif;'>";
        emailBody += "<div style='max-width: 600px; margin: auto; background: #fff; padding: 30px; border-radius: 10px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);'>";
        emailBody += "<h2 style='color: #333; margin-top: 0;'>Tài khoản đã được mở khóa</h2>";
        emailBody += "<p style='font-size: 16px; color: #555;'>Xin chào " + name + ",</p>";
        emailBody += "<p style='font-size: 16px; color: #555;'>Tài khoản của bạn trên <strong>Hệ thống PhyGen</strong> đã được mở khóa. Bạn có thể tiếp tục sử dụng dịch vụ như bình thường.</p>";
        if (!string.IsNullOrWhiteSpace(email))
        {
            emailBody += "<div style='text-align:center; margin: 28px 0;'>";
            emailBody += "<a href='" + email + "' style='display:inline-block; background-color:#28a745; color:#fff; text-decoration:none; padding:12px 20px; border-radius:6px; font-size:16px;'>Đăng nhập ngay</a>";
            emailBody += "</div>";
        }
        emailBody += "<p style='font-size: 14px; color: #888;'>Nếu bạn không thực hiện yêu cầu này, vui lòng liên hệ ngay với bộ phận hỗ trợ.</p>";
        emailBody += "<hr style='margin-top: 40px; border: none; border-top: 1px solid #eee;' />";
        emailBody += "<p style='font-size: 12px; color: #aaa;'>Trân trọng,<br/>Đội ngũ PhyGen</p>";
        emailBody += "</div>";
        emailBody += "</div>";
        return emailBody;
    }
}

