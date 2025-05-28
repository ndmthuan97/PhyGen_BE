using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Shared
{
    //The ApiResponse<T> class is used to package response data from an API in a uniform format.
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public IEnumerable<string>? Errors { get; set; }

        public ApiResponse(int statusCode, T? data = default, string message = "", IEnumerable<string>? errors = null)
        {
            StatusCode = statusCode;
            Data = data;
            Message = message;
            Errors = errors;
        }
    }
}
