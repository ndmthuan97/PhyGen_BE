using Microsoft.Extensions.Configuration;
using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.Application.Authentication.Interface;
using PhyGen.Application.Authentication.Responses;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Exceptions;
using PhyGen.Application.Authentication.Models.Requests;
using PhyGen.Insfrastructure.Persistence.DbContexts;
using PhyGen.Shared.Constants;
using PhyGen.Shared;
using Microsoft.EntityFrameworkCore;

namespace PhyGen.Insfrastructure.Service;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IEmailService _emailService;

    public AuthService(AppDbContext context, IJwtTokenGenerator jwtTokenGenerator, IEmailService emailService)
    {
        _context = context;
        _jwtTokenGenerator = jwtTokenGenerator;
        _emailService = emailService;
    }

    public async Task<AuthenticationResponse> RegisterAsync(RegisterDto dto)
    {
        if (_context.Users.Any(u => u.Email.ToLower() == dto.Email.ToLower()))
        {
            throw new AppException(StatusCode.EmailAlreadyExists);
        }

        var user = new ReserveUsers
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Phone = dto.PhoneNumber,
            Email = dto.Email,
            Password = dto.Password
        };
        string OTPText = Generaterandomnumber();
        await UpdateOtp(dto.Email, OTPText, "register");
        await SendOtpMail(dto.Email, OTPText, dto.Email);

        _context.ReserveUsers.Add(user);
        await _context.SaveChangesAsync();

        return new AuthenticationResponse
        {
            Email = user.Email,
        };
    }
    public async Task<AuthenticationResponse> ConfirmRegister(int userid, string email, string otptext)
    {
        AuthenticationResponse response = new AuthenticationResponse();

        // Bước 1: Kiểm tra OTP
        bool otpresponse = await ValidateOTP(email, otptext);
        if (!otpresponse)
        {
            response.Result = "fail";
            response.Message = "Invalid OTP or Expired";
            return response;
        }

        // Bước 2: Kiểm tra email đã tồn tại trong bảng Users chưa
        var existingUser = await this._context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (existingUser != null)
        {
            response.Result = "fail";
            response.Message = "Email already exists.";
            return response;
        }

        // Bước 3: Lấy thông tin tạm và thêm user mới
        var _tempdata = await this._context.ReserveUsers.FirstOrDefaultAsync(item => item.Email == email);
        if (_tempdata == null)
        {
            response.Result = "fail";
            response.Message = "Temporary user data not found.";
            return response;
        }

        var _user = new User()
        {
            Id = Guid.NewGuid(),
            FirstName = _tempdata.FirstName,
            LastName = _tempdata.LastName,
            Phone = _tempdata.Phone,
            Email = _tempdata.Email,
            Role = "User",
            Password = _tempdata.Password,           
        };

        await this._context.Users.AddAsync(_user);
        await this._context.SaveChangesAsync();

        response.Result = "pass";
        response.Message = "Registered successfully.";
        return response;
    }


    private async Task<bool> ValidateOTP(string username, string OTPText)
    {
        bool response = false;
        var _data = await this._context.EmailOtpManager.FirstOrDefaultAsync(item => item.Email == username
        && item.Otptext == OTPText && item.Expiration > DateTime.Now);
        if (_data != null)
        {
            response = true;
        }
        return response;
    }

    private async Task UpdateOtp(string username, string otptext, string otptype)
    {
        var _opt = new EmailOtpManager()
        {
            Email = username,
            Otptext = otptext,
            Expiration = DateTime.Now.AddMinutes(30),
            Createddate = DateTime.Now,
            Otptype = otptype
        };
        await this._context.EmailOtpManager.AddAsync(_opt);
        await this._context.SaveChangesAsync();
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

    public async Task ChangePasswordAsync(ChangePasswordDto dto)
    {
        var email = dto.Email.Trim().ToLower();
        var user = _context.Users.FirstOrDefault(u => u.Email.ToLower() == email);
        if (user == null)
            throw new AuthException(StatusCode.UserNotFound);

        if (user.Password != dto.CurrentPassword)
            throw new AuthException(StatusCode.InvalidPassword);

        // 3. Cập nhật mật khẩu mới (plain text)
        user.Password = dto.NewPassword;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
    public async Task<AuthenticationResponse> ForgetPassword(string email)
    {
        AuthenticationResponse response = new AuthenticationResponse();
        var _user = await this._context.Users.FirstOrDefaultAsync(item => item.Email == email);
        if (_user != null)
        {
            string otptext = Generaterandomnumber();
            await UpdateOtp(email, otptext, "forgetpassword");
            await SendOtpMail(_user.Email, otptext, _user.Email);
            response.Result = "pass";
            response.Message = "OTP sent";

        }
        else
        {
            response.Result = "fail";
            response.Message = "Invalid User";
        }
        return response;
    }

    public async Task<AuthenticationResponse> UpdatePassword(string email, string Password, string Otptext)
    {
        AuthenticationResponse response = new AuthenticationResponse();

        bool otpvalidation = await ValidateOTP(email, Otptext);
        if (otpvalidation)
        {
                var _user = await this._context.Users.FirstOrDefaultAsync(item => item.Email == email);
                if (_user != null)
                {
                    _user.Password = Password;
                    await _context.SaveChangesAsync();
                    response.Result = "pass";
                    response.Message = "Password changed";
                }
        }
        else
        {
            response.Result = "fail";
            response.Message = "Invalid OTP";
        }
        return response;
    }
    private string Generaterandomnumber()
    {
        Random random = new Random();
        string randomno = random.Next(100000, 1000000).ToString("D6");
        return randomno;
    }

    private async Task SendOtpMail(string useremail, string OtpText, string Name)
    {
        var mailrequest = new EmailRequest();
        mailrequest.Email = useremail;
        mailrequest.Subject = "Thanks for registering : OTP";
        mailrequest.Emailbody = GenerateEmailBody(Name, OtpText);
        await this._emailService.SendEmailAsync(mailrequest);
    }
    
    private static string GenerateEmailBody(string name, string otptext)
    {

        string emailBody = "<div style='width: 100%; background-color: #f4f4f4; padding: 20px 0; font-family: Arial, sans-serif;'>";
        emailBody += "<div style='max-width: 600px; margin: auto; background: #fff; padding: 30px; border-radius: 10px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);'>";
        emailBody += "<h2 style='color: #333;'>Hi " + name + ",</h2>";
        emailBody += "<p style='font-size: 16px; color: #555;'>Thanks for registering at <strong>PhyGen System</strong>.</p>";
        emailBody += "<p style='font-size: 16px; color: #555;'>Please enter the OTP code below to complete your registration:</p>";
        emailBody += "<div style='margin: 30px 0; text-align: center;'>";
        emailBody += "<span style='display: inline-block; background-color: #007BFF; color: #fff; padding: 12px 20px; font-size: 20px; border-radius: 5px; letter-spacing: 2px;'>" + otptext + "</span>";
        emailBody += "</div>";
        emailBody += "<p style='font-size: 14px; color: #888;'>This code will expire in 5 minutes.</p>";
        emailBody += "<hr style='margin-top: 40px; border: none; border-top: 1px solid #eee;'/>";
        emailBody += "<p style='font-size: 12px; color: #aaa;'>Regards,<br/>PhyGen Team</p>";
        emailBody += "</div>";
        emailBody += "</div>";

        return emailBody;
    }
}