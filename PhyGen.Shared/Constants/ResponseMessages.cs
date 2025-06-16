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
            // Curriculum messages
            { StatusCode.CurriculumNotFound, "Không tìm thấy chương trình học với mã được cung cấp." },
            { StatusCode.CurriculumSameName, "Chương trình học với tên này đã tồn tại." },
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
