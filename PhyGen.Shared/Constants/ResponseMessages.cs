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
            // General errors
            { StatusCode.ModelInvalid, "Mô hình không hợp lệ." },

            // Curriculum messages
            { StatusCode.CurriculumNotFound, "Không tìm thấy chương trình học với mã được cung cấp." },
            { StatusCode.CurriculumSameName, "Chương trình học với tên này đã tồn tại." },

            // Subject messages
            { StatusCode.SubjectNotFound, "Không tìm thấy môn học với mã được cung cấp." },
            { StatusCode.SubjectSameName, "Môn học với tên này đã tồn tại." },

            // SubjectBook messages
            { StatusCode.SubjectBookNotFound, "Không tìm thấy sách môn học với mã được cung cấp." },
            { StatusCode.SubjectBookSameName, "Sách môn học với tên này đã tồn tại." },
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
