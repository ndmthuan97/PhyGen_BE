using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Shared.Constants
{
    public static class ResponseMessages
    {
        private static readonly Dictionary<StatusCode, string> _messages = new Dictionary<StatusCode, string>
        {
            // Success messages
            { StatusCode.RequestProcessedSuccessfully, "Request processed successfully." },

            // Error messages
            { StatusCode.ModelInvalid, "Model is invalid. Please check the request body." },

            // Curriculum messages
            { StatusCode.CurriculumNotFound, "Curriculum with Id does not exist." },
            { StatusCode.CurriculumSameName, "Curriculum with the same name already exists." },
  
               { StatusCode.RequestProcessedSuccessfully, "Request processed successfully" },
               { StatusCode.ModelInvalid, "Model is invalid" },
               { StatusCode.UserAuthenticationFailed, "User authentication failed" },
               { StatusCode.EmailAlreadyExists, "Email already exists" },
               { StatusCode.RegisterFailed, "Register failed" },
               { StatusCode.RegisterSuccess, "Register succeeded" },
               { StatusCode.LoginFailed, "Login failed" },
               { StatusCode.LoginSuccess, "Login succeeded" },
               { StatusCode.UserNotFound, "User not found" },
               { StatusCode.InvalidPassword, "Invalid password" }
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
