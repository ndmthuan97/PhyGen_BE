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
            { StatusCode.RegisterSuccess, "Register succeeded." },
            { StatusCode.LoginSuccess, "Login succeeded." },
            { StatusCode.ChangedPasswordSuccess, "Password changed successfully." },
            { StatusCode.OtpSendSuccess, "Otp Send successfully." }, 

            // Error messages
            { StatusCode.ModelInvalid, "Model is invalid. Please check the request body." },
            { StatusCode.UserAuthenticationFailed, "User authentication failed" },
            { StatusCode.EmailAlreadyExists, "Email already existsaaa" },
            { StatusCode.RegisterFailed, "Register failed" },
            { StatusCode.LoginFailed, "Login failed" },
            { StatusCode.UserNotFound, "User not found" },
            { StatusCode.InvalidPassword, "Invalid password" },
            { StatusCode.EmailDoesNotExists, "Email Doesn't Exists" },
            { StatusCode.InvalidUser, "Invalid User." },
            { StatusCode.InvalidOtp, "Invalid Otp." }, 

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
