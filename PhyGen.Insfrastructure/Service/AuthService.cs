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
using Azure;
using Newtonsoft.Json.Linq;
using PhyGen.Application.Authentication.DTOs.Responses;
using AutoMapper;

namespace PhyGen.Insfrastructure.Service;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;

    public AuthService(AppDbContext context, IJwtTokenGenerator jwtTokenGenerator, IEmailService emailService, IMapper mapper)
    {
        _context = context;
        _jwtTokenGenerator = jwtTokenGenerator;
        _emailService = emailService;
        _mapper = mapper;
    }

    public async Task<AuthenticationResponse> RegisterAsync(RegisterDto dto)
    {
        var email = dto.Email.Trim().ToLower();
        if (_context.Users.Any(u => u.Email.ToLower() == email))
        {
            return new AuthenticationResponse
            {
                Email = dto.Email,
                StatusCode = StatusCode.EmailAlreadyExists
            };
        }

        // Hash password
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Phone = dto.PhoneNumber,
            Email = email,
            Password = hashedPassword,
            Role = "User",
            isConfirm = false
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

    public async Task<object> LoginAsync(LoginDto dto)
    {
        var email = dto.Email.Trim().ToLower();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email);

        if (user == null)
        {
            return new AuthenticationResponse
            {
                Email = email,
                StatusCode = StatusCode.EmailNotFound
            };
                
        }
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
        {
            return new AuthenticationResponse
            {
                Email = email,
                StatusCode = StatusCode.InvalidPassword
            };
        }

        if (!user.isConfirm)
        {
            // Gửi lại OTP nếu chưa xác thực
            string otp = Generaterandomnumber();
            await UpdateOtp(email, otp, "login");
            await SendOtpMail(email, otp, email, "login");

            return new AuthenticationResponse
            {
                Email = email,
                StatusCode = StatusCode.AccountNotConfirmed
            };
        }

        // Đã xác thực => đăng nhập thành công
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new LoginResponse
        {
            Response = new AuthenticationResponse
            {
                Email = email,
                StatusCode = StatusCode.LoginSuccess
            },
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
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

            return new LoginResponse
            {
                Response = new AuthenticationResponse
                {
                    Email = email,
                    StatusCode = StatusCode.ConfirmSuccess
                },
                Token = _jwtTokenGenerator.GenerateToken(user),
                Role = user.Role
            };
            }

    public async Task<AuthenticationResponse> ChangePasswordAsync(ChangePasswordDto dto)
    {
        var email = dto.Email.Trim().ToLower();
        var user = _context.Users.FirstOrDefault(u => u.Email.ToLower() == email);
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

        // 3. Cập nhật mật khẩu mới (plain text)
        user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return new AuthenticationResponse
        {
            Email = email,
            StatusCode = StatusCode.ChangedPasswordSuccess
        }; 

    }
    public async Task<AuthenticationResponse> ForgetPassword(string email)
    {
        var _user = await this._context.Users.FirstOrDefaultAsync(item => item.Email == email);
        if (_user != null)
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
                StatusCode = StatusCode.InvalidUser
            };
        }
    }

    public async Task<AuthenticationResponse> UpdatePassword(string email, string Password, string Otptext)
    {
        bool otpvalidation = await ValidateOTP(email, Otptext);
        if (otpvalidation)
        {
            var _user = await this._context.Users.FirstOrDefaultAsync(item => item.Email == email);           
            if (_user != null)
            {
                _user.Password = BCrypt.Net.BCrypt.HashPassword(Password);
                await _context.SaveChangesAsync();
                return new AuthenticationResponse
                {
                    Email = email,
                    StatusCode = StatusCode.ChangedPasswordSuccess
                };
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
        return new AuthenticationResponse
        {
            Email = email,
            StatusCode = StatusCode.InvalidOtp
        };
    }
    private string Generaterandomnumber()
    {
        Random random = new Random();
        string randomno = random.Next(100000, 1000000).ToString("D6");
        return randomno;
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

    private static string GenerateForgetPasswordEmail(string name, string otptext)
    {
        string emailBody = "<div style='width: 100%; background-color: #f4f4f4; padding: 20px 0; font-family: Arial, sans-serif;'>";
        emailBody += "<div style='max-width: 600px; margin: auto; background: #fff; padding: 30px; border-radius: 10px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);'>";
        emailBody += "<h2 style='color: #333;'>Hi " + name + ",</h2>";
        emailBody += "<p style='font-size: 16px; color: #555;'>We received a request to reset your password for your <strong>PhyGen System</strong> account.</p>";
        emailBody += "<p style='font-size: 16px; color: #555;'>Please use the OTP code below to proceed with resetting your password:</p>";
        emailBody += "<div style='margin: 30px 0; text-align: center;'>";
        emailBody += "<span style='display: inline-block; background-color: #28a745; color: #fff; padding: 12px 20px; font-size: 20px; border-radius: 5px; letter-spacing: 2px;'>" + otptext + "</span>";
        emailBody += "</div>";
        emailBody += "<p style='font-size: 14px; color: #888;'>This code will expire in 5 minutes. If you didn't request a password reset, please ignore this email.</p>";
        emailBody += "<hr style='margin-top: 40px; border: none; border-top: 1px solid #eee;' />";
        emailBody += "<p style='font-size: 12px; color: #aaa;'>Regards,<br/>PhyGen Team</p>";
        emailBody += "</div>";
        emailBody += "</div>";

        return emailBody;
    }
    private static string GenerateLoginConfirmEmail(string name, string otptext)
    {
        string emailBody = "<div style='width: 100%; background-color: #f4f4f4; padding: 20px 0; font-family: Arial, sans-serif;'>";
        emailBody += "<div style='max-width: 600px; margin: auto; background: #fff; padding: 30px; border-radius: 10px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);'>";
        emailBody += "<h2 style='color: #333;'>Welcome back, " + name + "!</h2>";
        emailBody += "<p style='font-size: 16px; color: #555;'>We're glad to see you again at <strong>PhyGen System</strong>.</p>";
        emailBody += "<p style='font-size: 16px; color: #555;'>Please verify the OTP code below to continue accessing the system:</p>";
        emailBody += "<div style='margin: 30px 0; text-align: center;'>";
        emailBody += "<span style='display: inline-block; background-color: #17a2b8; color: #fff; padding: 12px 20px; font-size: 20px; border-radius: 5px; letter-spacing: 2px;'>" + otptext + "</span>";
        emailBody += "</div>";
        emailBody += "<p style='font-size: 14px; color: #888;'>This OTP code will expire in 5 minutes. If you did not request this, please ignore this email.</p>";
        emailBody += "<hr style='margin-top: 40px; border: none; border-top: 1px solid #eee;' />";
        emailBody += "<p style='font-size: 12px; color: #aaa;'>Best regards,<br/>The PhyGen Team</p>";
        emailBody += "</div>";
        emailBody += "</div>";

        return emailBody;
    }

}