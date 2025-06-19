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

            // Curriculum messages
            { StatusCode.CurriculumNotFound, "Không tìm thấy chương trình học với mã được cung cấp." },
            { StatusCode.CurriculumSameName, "Chương trình học với tên này đã tồn tại." },

            // Subject messages
            { StatusCode.SubjectNotFound, "Không tìm thấy môn học với mã được cung cấp." },
            { StatusCode.SubjectSameName, "Môn học với tên này đã tồn tại." },

            // SubjectBook messages
            { StatusCode.SubjectBookNotFound, "Không tìm thấy sách môn học với mã được cung cấp." },
            { StatusCode.SubjectBookAlreadyExist, "Sách môn học này đã tồn tại." },

            // Chapter messages
            { StatusCode.ChapterNotFound, "Không tìm thấy chương với mã được cung cấp." },
            { StatusCode.ChapterAlreadyExist, "Chương này đã tồn tại." },

            // Topic messages
            { StatusCode.TopicNotFound, "Không tìm thấy chủ đề với mã được cung cấp." },
            { StatusCode.TopicAlreadyExist, "Chủ đề này đã tồn tại." },

            // ContentFlow messages
            { StatusCode.ContentFlowNotFound, "Không tìm thấy luồng nội dung với mã được cung cấp." },
            { StatusCode.ContentFlowAlreadyExist, "Luồng nội dung đã tồn tại." },

            // ContentItem messages
            { StatusCode.ContentItemNotFound, "Không tìm thấy mục nội dung với mã được cung cấp." },
            { StatusCode.ContentItemAlreadyExist, "Mục nội dung đã tồn tại." },

            // ExamCategory messages
            { StatusCode.ExamCategoryNotFound, "Không tìm thấy danh mục bài kiểm tra với mã được cung cấp." },
            { StatusCode.ExamCategorySameName, "Danh mục bài kiểm tra với tên này đã tồn tại." },

            // ExamCategoryChapter messages
            { StatusCode.ExamCategoryChapterNotFound, "Không tìm thấy danh mục chương bài kiểm tra với mã được cung cấp." },
            { StatusCode.ExamCategoryChapterAlreadyExist, "Danh mục chương bài kiểm tra đã tồn tại." },
            
            // ContentItemExamCategory messages
            { StatusCode.ContentItemExamCategoryNotFound, "Không tìm thấy danh mục bài kiểm tra của mục nội dung với mã được cung cấp." },
            { StatusCode.ContentItemExamCategoryAlreadyExist, "Danh mục bài kiểm tra của mục nội dung đã tồn tại." },

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
            { StatusCode.AccountLocked, "Tài khoản đã bị khóa." },

             // Error messages
            { StatusCode.ModelInvalid, "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại nội dung yêu cầu." },

            // Notification messages
            { StatusCode.NotifcationNotFound, "Không tìm thấy thông báo với mã được cung cấp." },
            { StatusCode.NotifcationSend, "Thông báo này đã được gửi." },
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
