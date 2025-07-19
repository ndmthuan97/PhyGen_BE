using Microsoft.Extensions.Configuration;
using PhyGen.Application.Authentication.DTOs.Dtos;
using PhyGen.Application.Authentication.Interface;
using PhyGen.Application.Authentication.Responses;
using PhyGen.Domain.Entities;
using PhyGen.Domain.Exceptions;
using PhyGen.Application.Authentication.Models.Requests;
using PhyGen.Infrastructure.Persistence.DbContexts;
using PhyGen.Shared.Constants;
using PhyGen.Shared;
using Microsoft.EntityFrameworkCore;
using Azure;
using Newtonsoft.Json.Linq;
using PhyGen.Application.Authentication.DTOs.Responses;
using AutoMapper;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace PhyGen.Infrastructure.Service;

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
        var email = dto.Email.Trim().ToLower();
        if (await _context.Users.AnyAsync(u => u.Email.ToLower() == email))
        {
            return new AuthenticationResponse
            {
                Email = dto.Email,
                StatusCode = StatusCode.EmailAlreadyExists
            };
        }

        // Định dạng mật khẩu
        var password = dto.Password;
        var passwordRegex = new Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?""|<>]).{8,}$", RegexOptions.Compiled, TimeSpan.FromSeconds(1));
        if (!passwordRegex.IsMatch(password))
        {
            return new AuthenticationResponse
            {
                Email = dto.Email,
                StatusCode = StatusCode.InvalidPasswordFormat, // Thêm enum nếu chưa có                
            };
        }

        if (dto.Password != dto.ConfirmPassword)
        {
            return new AuthenticationResponse
            {
                Email = dto.Email,
                StatusCode = StatusCode.PasswordMismatch
            };
        }

        // Hash password
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            Password = hashedPassword,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Gender = dto.Gender,
            Role = "User",
            isConfirm = false,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);

        string otp = Generaterandomnumber();
        await UpdateOtp(dto.Email, otp, "register");
        await SendOtpMail(dto.Email, otp, dto.Email, "register");

        await _context.SaveChangesAsync();

        return new AuthenticationResponse
        {
            Email = dto.Email,
            StatusCode = StatusCode.RegisterSuccess
        };
    }

    public async Task<AuthenticationResponse> ConfirmRegister(string email, string otptext)
    {
        email = email.ToLower();

        // Bước 1: Tìm người dùng trong bảng Users
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email);
        if (user == null)
        {
            return new AuthenticationResponse
            {
                Email = email,
                StatusCode = StatusCode.EmailAlreadyExists
            };
        }

        // Bước 2: Kiểm tra OTP
        bool otpresponse = await ValidateOTP(email, otptext);
        if (!otpresponse)
        {
            return new AuthenticationResponse
            {
                Email = email,
                StatusCode = StatusCode.UserAuthenticationFailed
            };
        }

        // Bước 3: Kiểm tra xem đã confirm hay chưa
        if (user.isConfirm)
        {
            return new AuthenticationResponse
            {
                Email = email,
                StatusCode = StatusCode.AccountNotConfirmed
            };
        }

        // Bước 4: Cập nhật trạng thái isConfirm
        user.isConfirm = true;
        user.IsActive = true;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return new AuthenticationResponse
        {
            Email = email,
            StatusCode = StatusCode.RegisterSuccess
        };
    }

    private async Task<bool> ValidateOTP(string username, string OTPText)
    {
        bool response = false;
        var _data = await _context.EmailOtpManagers.FirstOrDefaultAsync(item => item.Email == username
        && item.Otptext == OTPText && item.Expiration > DateTime.UtcNow);
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
            Expiration = DateTime.UtcNow.AddMinutes(30),
            Createddate = DateTime.UtcNow,
            Otptype = otptype
        };
        await _context.EmailOtpManagers.AddAsync(_opt);
        await _context.SaveChangesAsync();
    }
    public async Task<object> LoginAsync(LoginDto dto, string token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            return await HandleLoginWithTokenAsync(dto, token);
        }
        return await HandleLoginWithEmailsAsync(dto);
    }

    private async Task<object> HandleLoginWithTokenAsync(LoginDto dto, string accessToken)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(accessToken);

            var email = token.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            var userMetadataJson = token.Claims.FirstOrDefault(c => c.Type == "user_metadata")?.Value;

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email.Trim().ToLower() == email.Trim().ToLower());

            if (existingUser != null)
            {
                return await HandleExistingUserLoginAsync(existingUser, email);
            }

            return await HandleNewTokenUserRegistrationAsync(email, userMetadataJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Token invalid or parsing error: {ex.Message}");

            return new AuthenticationResponse
            {
                Email = dto.Email,
                StatusCode = StatusCode.InvalidToken
            };
        }
    }

    private async Task<object> HandleExistingUserLoginAsync(User user, string email)
    {
        if (!user.IsActive)
        {
            return new AuthenticationResponse { Email = email, StatusCode = StatusCode.AccountLocked };
        }

        if (!string.IsNullOrEmpty(user.Password))
        {
            return new AuthenticationResponse { Email = email, StatusCode = StatusCode.MustLoginWithEmailPassword };
        }

        user.LastLogin = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return new LoginResponse
        {
            Response = new AuthenticationResponse { Email = email, FirstName = user.FirstName, LastName = user.LastName, PhotoUrl = user.photoURL ,StatusCode = StatusCode.LoginSuccess },
            Role = user.Role,
            Token = _jwtTokenGenerator.GenerateToken(user)
        };
    }

    private async Task<object> HandleNewTokenUserRegistrationAsync(string email, string metadataJson)
    {
        var metadata = JsonSerializer.Deserialize<UserMetadata>(metadataJson);
        var fullName = metadata?.full_name?.Trim() ?? "";
        var avatarUrl = metadata?.avatar_url;

        var (firstName, lastName) = ParseFullName(fullName);

        var newUser = new User
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            photoURL = avatarUrl,
            CreatedAt = DateTime.UtcNow,
            isConfirm = true,
            IsActive = true,
            Role = "User",
            LastLogin = DateTime.UtcNow
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return new LoginResponse
        {
            Response = new AuthenticationResponse { Email = email, FirstName = newUser.FirstName, LastName = newUser.LastName ,StatusCode = StatusCode.LoginSuccess },
            Role = newUser.Role,
            Token = _jwtTokenGenerator.GenerateToken(newUser)

        };
    }

    private (string FirstName, string LastName) ParseFullName(string fullName)
    {
        var parts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length > 1
            ? (parts[0], string.Join(" ", parts.Skip(1)))
            : (fullName, "");
    }

    private async Task<object> HandleLoginWithEmailsAsync(LoginDto dto)
    {
        var email = dto.Email.Trim().ToLower();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email);

        if (user == null)
        {
            return new AuthenticationResponse { Email = email, StatusCode = StatusCode.EmailNotFound };
        }

        if (!user.IsActive)
        {
            return new AuthenticationResponse { Email = email, StatusCode = StatusCode.AccountLocked };
        }

        if (string.IsNullOrEmpty(user.Password))
        {
            return new AuthenticationResponse { Email = email, StatusCode = StatusCode.MustLoginWithGoogle };
        }

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
        {
            return new AuthenticationResponse { Email = email, StatusCode = StatusCode.InvalidPassword };
        }

        if (!user.isConfirm)
        {
            var otp = Generaterandomnumber();
            await UpdateOtp(email, otp, "login");
            await SendOtpMail(email, otp, email, "login");

            return new AuthenticationResponse { Email = email, StatusCode = StatusCode.AccountNotConfirmed };
        }

        user.LastLogin = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new LoginResponse
        {
            Response = new AuthenticationResponse {Email = email, FirstName = user.FirstName, LastName = user.LastName, PhotoUrl = user.photoURL, StatusCode = StatusCode.LoginSuccess },
            Token = token,
            Role = user.Role
        };
    }

    public async Task<object> ConfirmLogin(string email, string otpText)
    {
        email = email.ToLower();

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email);
        if (user == null)
        {
            return new AuthenticationResponse
            {
                Email = email,
                StatusCode = StatusCode.EmailNotFound
            };
        }

        bool otpValid = await ValidateOTP(email, otpText);
        if (!otpValid)
        {
            return new AuthenticationResponse
            {
                Email = email,
                StatusCode = StatusCode.UserAuthenticationFailed
            };       
        }

        if (user.isConfirm)
        {
            return new AuthenticationResponse
            {
                Email = email,
                StatusCode = StatusCode.AlreadyConfirmed
            };
        }
        user.isConfirm = true;
        user.IsActive = true;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

            return new LoginResponse
            {
                Response = new AuthenticationResponse
                {
                    Email = email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    StatusCode = StatusCode.ConfirmSuccess
                },
                Token = _jwtTokenGenerator.GenerateToken(user),
                Role = user.Role
            };
    }

    public async Task<AuthenticationResponse> ChangePasswordAsync(ChangePasswordDto dto)
    {
        var email = dto.Email.Trim().ToLower();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email);
        if (user == null)
        {
            return new AuthenticationResponse
            {
                Email = email,
                StatusCode = StatusCode.EmailDoesNotExists
            };
        }

        bool verify = BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.Password);
        if (!verify)
        {
            return new AuthenticationResponse
            {
                Email = email,
                StatusCode = StatusCode.InvalidPassword
            };
        }

        // Kiểm tra định dạng mật khẩu mới
        var passwordRegex = new Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?""|<>]).{8,}$",
            RegexOptions.Compiled,
            TimeSpan.FromSeconds(1));
        if (!passwordRegex.IsMatch(dto.NewPassword))
        {
            return new AuthenticationResponse
            {
                Email = email,
                StatusCode = StatusCode.InvalidPasswordFormat,
            };
        }

        // (Tùy chọn) Kiểm tra NewPassword có trùng ConfirmNewPassword không
        if (dto.NewPassword != dto.ConfirmNewPassword)
        {
            return new AuthenticationResponse
            {
                Email = email,
                StatusCode = StatusCode.PasswordMismatch,
            };
        }

        // Cập nhật mật khẩu mới
        user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        await _context.SaveChangesAsync();

        return new AuthenticationResponse
        {
            Email = email,
            StatusCode = StatusCode.ChangedPasswordSuccess
        };
    }

    public async Task<AuthenticationResponse> ForgetPassword(string email)
    {
        var _user = await _context.Users.FirstOrDefaultAsync(item => item.Email == email);

        if (_user != null)
        {
            if (!string.IsNullOrEmpty(_user.Password))
            {
                string otptext = Generaterandomnumber();
                await UpdateOtp(email, otptext, "forgetpassword");
                await SendOtpMail(_user.Email, otptext, _user.Email, "forgetpassword");

                return new AuthenticationResponse
                {
                    Email = email,
                    StatusCode = StatusCode.OtpSendSuccess
                };
            }
            else
            {
                return new AuthenticationResponse
                {
                    Email = email,
                    StatusCode = StatusCode.MustLoginWithGoogle,
                };
            }
        }
        else
        {
            return new AuthenticationResponse
            {
                Email = email,
                StatusCode = StatusCode.InvalidUser
            };
        }
    }


    public async Task<AuthenticationResponse> UpdatePassword(string email, string password, string otpText)
    {
        bool otpValidation = await ValidateOTP(email, otpText);
        var _user = await _context.Users.FirstOrDefaultAsync(item => item.Email == email);

        if (_user == null)
        {
            return new AuthenticationResponse
            {
                Email = email,
                StatusCode = StatusCode.InvalidUser
            };
        }

        if (!otpValidation)
        {
            return new AuthenticationResponse
            {
                Email = email,
                StatusCode = StatusCode.InvalidOtp
            };
        }

        // Kiểm tra định dạng mật khẩu
        var passwordRegex = new Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?""|<>]).{8,}$", 
            RegexOptions.Compiled,
            TimeSpan.FromSeconds(1));
        if (!passwordRegex.IsMatch(password))
        {
            return new AuthenticationResponse
            {
                Email = email,
                StatusCode = StatusCode.InvalidPasswordFormat
            };
        }

        // Cập nhật mật khẩu
        _user.Password = BCrypt.Net.BCrypt.HashPassword(password);
        await _context.SaveChangesAsync();

        return new AuthenticationResponse
        {
            Email = email,
            StatusCode = StatusCode.ChangedPasswordSuccess
        };
    }

    private static string Generaterandomnumber()
    {
        int number = RandomNumberGenerator.GetInt32(100000, 1000000);
        return number.ToString("D6");
    }

    private async Task SendOtpMail(string useremail, string OtpText, string Name, string type)
    {
        var mailrequest = new EmailRequest();
        mailrequest.Email = useremail;

        if (type == "register")
        {
            mailrequest.Subject = "Thanks for registering : OTP";
            mailrequest.Emailbody = GenerateEmailBody(Name, OtpText);
        }
        else if (type == "forgetpassword")
        {
            mailrequest.Subject = "Password Reset Request : OTP";
            mailrequest.Emailbody = GenerateForgetPasswordEmail(Name, OtpText);
        }
        else if (type == "login")
        {
            mailrequest.Subject = "Xác thực đăng nhập: OTP";
            mailrequest.Emailbody = GenerateLoginConfirmEmail(Name, OtpText);
        }

        await _emailService.SendEmailAsync(mailrequest);
    }
    public class UserMetadata
    {
        public string avatar_url { get; set; }
        public string full_name { get; set; }
    }
    private static string GenerateEmailBody(string name, string otptext)
    {
        string emailBody = "<div style='width: 100%; background-color: #f4f4f4; padding: 20px 0; font-family: Arial, sans-serif;'>";
        emailBody += "<div style='max-width: 600px; margin: auto; background: #fff; padding: 30px; border-radius: 10px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);'>";
        emailBody += "<h2 style='color: #333;'>Xin chào " + name + ",</h2>";
        emailBody += "<p style='font-size: 16px; color: #555;'>Cảm ơn bạn đã đăng ký tài khoản tại <strong>Hệ thống PhyGen</strong>.</p>";
        emailBody += "<p style='font-size: 16px; color: #555;'>Vui lòng nhập mã OTP dưới đây để hoàn tất quá trình đăng ký:</p>";
        emailBody += "<div style='margin: 30px 0; text-align: center;'>";
        emailBody += "<span style='display: inline-block; background-color: #007BFF; color: #fff; padding: 12px 20px; font-size: 20px; border-radius: 5px; letter-spacing: 2px;'>" + otptext + "</span>";
        emailBody += "</div>";
        emailBody += "<p style='font-size: 14px; color: #888;'>Mã OTP này sẽ hết hạn sau 5 phút.</p>";
        emailBody += "<hr style='margin-top: 40px; border: none; border-top: 1px solid #eee;' />";
        emailBody += "<p style='font-size: 12px; color: #aaa;'>Trân trọng,<br/>Đội ngũ PhyGen</p>";
        emailBody += "</div>";
        emailBody += "</div>";

        return emailBody;
    }

    private static string GenerateForgetPasswordEmail(string name, string otptext)
    {
        string emailBody = "<div style='width: 100%; background-color: #f4f4f4; padding: 20px 0; font-family: Arial, sans-serif;'>";
        emailBody += "<div style='max-width: 600px; margin: auto; background: #fff; padding: 30px; border-radius: 10px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);'>";
        emailBody += "<h2 style='color: #333;'>Xin chào " + name + ",</h2>";
        emailBody += "<p style='font-size: 16px; color: #555;'>Chúng tôi đã nhận được yêu cầu đặt lại mật khẩu cho tài khoản <strong>Hệ thống PhyGen</strong> của bạn.</p>";
        emailBody += "<p style='font-size: 16px; color: #555;'>Vui lòng sử dụng mã OTP dưới đây để tiếp tục quá trình đặt lại mật khẩu:</p>";
        emailBody += "<div style='margin: 30px 0; text-align: center;'>";
        emailBody += "<span style='display: inline-block; background-color: #28a745; color: #fff; padding: 12px 20px; font-size: 20px; border-radius: 5px; letter-spacing: 2px;'>" + otptext + "</span>";
        emailBody += "</div>";
        emailBody += "<p style='font-size: 14px; color: #888;'>Mã OTP này sẽ hết hạn sau 5 phút. Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này.</p>";
        emailBody += "<hr style='margin-top: 40px; border: none; border-top: 1px solid #eee;' />";
        emailBody += "<p style='font-size: 12px; color: #aaa;'>Trân trọng,<br/>Đội ngũ PhyGen</p>";
        emailBody += "</div>";
        emailBody += "</div>";

        return emailBody;
    }

    private static string GenerateLoginConfirmEmail(string name, string otptext)
    {
        string emailBody = "<div style='width: 100%; background-color: #f4f4f4; padding: 20px 0; font-family: Arial, sans-serif;'>";
        emailBody += "<div style='max-width: 600px; margin: auto; background: #fff; padding: 30px; border-radius: 10px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);'>";
        emailBody += "<h2 style='color: #333;'>Chào mừng trở lại, " + name + "!</h2>";
        emailBody += "<p style='font-size: 16px; color: #555;'>Rất vui được gặp lại bạn tại <strong>Hệ thống PhyGen</strong>.</p>";
        emailBody += "<p style='font-size: 16px; color: #555;'>Vui lòng xác thực mã OTP dưới đây để tiếp tục đăng nhập:</p>";
        emailBody += "<div style='margin: 30px 0; text-align: center;'>";
        emailBody += "<span style='display: inline-block; background-color: #17a2b8; color: #fff; padding: 12px 20px; font-size: 20px; border-radius: 5px; letter-spacing: 2px;'>" + otptext + "</span>";
        emailBody += "</div>";
        emailBody += "<p style='font-size: 14px; color: #888;'>Mã OTP này sẽ hết hạn sau 5 phút. Nếu bạn không yêu cầu đăng nhập, vui lòng bỏ qua email này.</p>";
        emailBody += "<hr style='margin-top: 40px; border: none; border-top: 1px solid #eee;' />";
        emailBody += "<p style='font-size: 12px; color: #aaa;'>Trân trọng,<br/>Đội ngũ PhyGen</p>";
        emailBody += "</div>";
        emailBody += "</div>";

        return emailBody;
    }
}