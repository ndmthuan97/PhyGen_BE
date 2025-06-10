using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Shared.Constants
{
    public static class ResponseMessages
    {
        private static readonly Dictionary<StatusCode, string> _messages = new()
        {
            // Success messages
            { StatusCode.RequestProcessedSuccessfully, "Yêu cầu đã được xử lý thành công." },
            { StatusCode.RegisterSuccess, "Đăng ký thành công." },
            { StatusCode.LoginSuccess, "Đăng nhập thành công." },
            { StatusCode.ChangedPasswordSuccess, "Đổi mật khẩu thành công." },
            { StatusCode.OtpSendSuccess, "Mã OTP đã được gửi thành công." },

            // Error messages
            { StatusCode.ModelInvalid, "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại nội dung yêu cầu." },
            
            // Authentication error messages
            { StatusCode.UserAuthenticationFailed, "Xác thực người dùng không thành công." },
            { StatusCode.UserNotFound, "Không tìm thấy người dùng với mã được cung cấp." },
            { StatusCode.RegisterFailed, "Đăng ký không thành công. Vui lòng thử lại." },
            { StatusCode.LoginFailed, "Đăng nhập không thành công. Vui lòng kiểm tra thông tin đăng nhập." },
            { StatusCode.EmailAlreadyExists, "Email đã tồn tại trong hệ thống." },
            { StatusCode.EmailDoesNotExists, "Email không tồn tại trong hệ thống." },
            { StatusCode.InvalidUser, "Người dùng không hợp lệ." },
            { StatusCode.InvalidPassword, "Mật khẩu không hợp lệ." },
            { StatusCode.InvalidOtp, "Mã OTP không hợp lệ." },
            { StatusCode.EmailNotFound, "Không tìm thấy email trong hệ thống." },
            { StatusCode.AccountNotConfirmed, "Tài khoản chưa được xác nhận." },
            { StatusCode.AlreadyConfirmed, "Tài khoản đã được xác nhận trước đó." },
            { StatusCode.ConfirmSuccess, "Xác nhận tài khoản thành công." },
            { StatusCode.PasswordMismatch, "Mật khẩu không khớp." },
            { StatusCode.InvalidPasswordFormat, "Mật khẩu phải có 8 ký tự gồm ký tự in hoa, số và ký tự đặc biệt" },
            { StatusCode.InvalidGoogleToken, "Mã thông báo Google không hợp lệ." },
            { StatusCode.InvalidToken, "Mã thông báo không hợp lệ." },
            { StatusCode.MustLoginWithEmailPassword, "Bạn phải đăng nhập bằng email và mật khẩu." },
            { StatusCode.MustLoginWithGoogle, "Bạn phải đăng nhập bằng google." },

            // Curriculum messages
            { StatusCode.CurriculumNotFound, "Không tìm thấy chương trình học với mã được cung cấp." },
            { StatusCode.CurriculumSameName, "Chương trình học với tên này đã tồn tại." },

            // Chapter messages
            { StatusCode.ChapterNotFound, "Không tìm thấy chương với mã được cung cấp." },
            { StatusCode.ChapterSameName, "Chương với tên này đã tồn tại." },

            // Subject messages
            { StatusCode.SubjectNotFound, "Không tìm thấy môn học với mã được cung cấp." },
            { StatusCode.SubjectSameName, "Môn học với tên này đã tồn tại." },

            // Subject Curriculum messages
            { StatusCode.SubjectCurriculumNotFound, "Không tìm thấy chương trình học của môn học với mã được cung cấp." },

            // Chapter Unit messages
            { StatusCode.ChapterUnitNotFound, "Không tìm thấy đơn vị chương với mã được cung cấp." },
            { StatusCode.ChapterUnitSameName, "Đơn vị chương với tên này đã tồn tại." },
        };

        public static string GetMessage(StatusCode code)
        {
            if (_messages.TryGetValue(code, out var message))
            {
                return message;
            }

            throw new ArgumentException($"No message found for status code: {code}", nameof(code));
        }
    }
}
