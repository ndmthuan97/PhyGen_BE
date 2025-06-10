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

            // Error messages
            { StatusCode.ModelInvalid, "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại nội dung yêu cầu." },

            // Authentication error messages
            { StatusCode.UserAuthenticationFailed, "Xác thực người dùng không thành công." },
            { StatusCode.UserNotFound, "Không tìm thấy người dùng với mã được cung cấp." },

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

            // Exam messages
            { StatusCode.ExamNotFound, "Không tìm thấy bài kiểm tra với mã được cung cấp." },
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
