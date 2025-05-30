using System;
using System.Collections.Generic;

namespace PhyGen.Shared.Constants
{
    public static class ResponseMessages
    {
        private static readonly Dictionary<StatusCode, string> _messages = new()
        {
            // Success messages
            { StatusCode.RequestProcessedSuccessfully, "Request processed successfully." },

            // Authentication success messages
            { StatusCode.RegisterSuccess, "Register succeeded" },
            { StatusCode.LoginSuccess, "Login succeeded" },

            // Error messages
            { StatusCode.ModelInvalid, "Model is invalid. Please check the request body." },


            // Authentication error messages
            { StatusCode.UserAuthenticationFailed, "User authentication failed" },
            { StatusCode.EmailAlreadyExists, "Email already exists" },
            { StatusCode.RegisterFailed, "Register failed" },
            { StatusCode.LoginFailed, "Login failed" },
            { StatusCode.UserNotFound, "User not found" },
            { StatusCode.InvalidPassword, "Invalid password" },

            // Curriculum messages
            { StatusCode.CurriculumNotFound, "Curriculum with Id does not exist." },
            { StatusCode.CurriculumSameName, "Curriculum with the same name already exists." }
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
