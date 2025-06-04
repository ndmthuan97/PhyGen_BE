using System;
using System.Collections.Generic;

namespace PhyGen.Shared.Constants
{
    public static class ResponseMessages
    {
        private static readonly Dictionary<StatusCode, string> _messages = new()
        {
            // Success messages
            { StatusCode.RequestProcessedSuccessfully, "Yêu cầu đã được xử lý thành công." },
            { StatusCode.RegisterSuccess, "Đăng ký tài khoản thành công." },
            { StatusCode.LoginSuccess, "Đăng nhập thành công." },
            { StatusCode.ChangedPasswordSuccess, "Đổi mật khẩu thành công." },
            { StatusCode.OtpSendSuccess, "Mã OTP đã được gửi thành công." },

            // Error messages
            { StatusCode.ModelInvalid, "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại nội dung yêu cầu." },

            // Authentication error messages
            { StatusCode.UserAuthenticationFailed, "Xác thực người dùng không thành công." },
            { StatusCode.EmailAlreadyExists, "Email này đã được sử dụng." },
            { StatusCode.RegisterFailed, "Đăng ký tài khoản thất bại." },
            { StatusCode.LoginFailed, "Đăng nhập thất bại. Vui lòng kiểm tra thông tin đăng nhập." },
            { StatusCode.UserNotFound, "Không tìm thấy người dùng." },
            { StatusCode.InvalidPassword, "Mật khẩu không chính xác." },
            { StatusCode.EmailDoesNotExists, "Email không tồn tại trong hệ thống." },
            { StatusCode.InvalidUser, "Tài khoản không hợp lệ." },
            { StatusCode.InvalidOtp, "Mã OTP không chính xác." },
            { StatusCode.PasswordMismatch, "Mật khẩu và xác nhận mật khẩu không khớp." },
            { StatusCode.EmailNotFound, "Không tìm thấy email." },
            { StatusCode.AccountNotConfirmed, "Tài khoản chưa được xác nhận." },
            { StatusCode.AlreadyConfirmed, "Tài khoản đã được xác nhận trước đó." },
            { StatusCode.ConfirmSuccess, "Xác nhận tài khoản thành công." },

            // Curriculum messages
            { StatusCode.CurriculumNotFound, "Không tìm thấy chương trình học với mã được cung cấp." },
            { StatusCode.CurriculumSameName, "Chương trình học với tên này đã tồn tại." },

            // Chapter messages
            { StatusCode.ChapterNotFound, "Không tìm thấy chương với mã được cung cấp." },
            { StatusCode.ChapterSameName, "Chương với tên này đã tồn tại." },

            // Chapter Unit messages
            { StatusCode.ChapterUnitNotFound, "Không tìm thấy đơn vị chương với mã được cung cấp." },
            { StatusCode.ChapterUnitSameName, "Đơn vị chương với tên này đã tồn tại." },

            // Book messages
            { StatusCode.BookNotFound, "Không tìm thấy sách với mã được cung cấp." },
            { StatusCode.BookSameName, "Sách với tên này đã tồn tại." },

            // Book Series messages
            { StatusCode.BookSeriesNotFound, "Không tìm thấy bộ sách với mã được cung cấp." },
            { StatusCode.BookSeriesSameName, "Bộ sách với tên này đã tồn tại." },

            // Book Detail messages
            { StatusCode.BookDetailNotFound, "Không tìm thấy thông tin chi tiết sách với mã được cung cấp." },

            // Question messages
            { StatusCode.QuestionNotFound, "Không tìm thấy câu hỏi với mã được cung cấp." },
            { StatusCode.QuestionSameContent, "Câu hỏi này đã tồn tại trong hệ thống." },

            // Answer messages
            { StatusCode.AnswerNotFound, "Không tìm thấy câu trả lời với mã được cung cấp." },
            { StatusCode.AnswerSameContent, "Câu trả lời này đã tồn tại trong hệ thống." },

            // Exam messages
            { StatusCode.ExamNotFound, "Không tìm thấy đề thi với mã được cung cấp." },
            { StatusCode.ExamSameTitle, "Đề thi với tiêu đề này đã tồn tại." },

            // Matrix messages
            { StatusCode.MatrixNotFound, "Không tìm thấy ma trận với mã được cung cấp." },
            { StatusCode.MatrixSameName, "Ma trận với tên này đã tồn tại." },

            // Matrix Detail messages
            { StatusCode.MatrixDetailNotFound, "Không tìm thấy chi tiết ma trận với mã được cung cấp." },
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
