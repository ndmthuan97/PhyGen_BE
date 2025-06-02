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


            // Authentication error messages
            { StatusCode.UserAuthenticationFailed, "User authentication failed" },
            { StatusCode.EmailAlreadyExists, "Email already exists" },
            { StatusCode.RegisterFailed, "Register failed" },
            { StatusCode.LoginFailed, "Login failed" },
            { StatusCode.UserNotFound, "User not found" },
            { StatusCode.InvalidPassword, "Invalid password" },
            { StatusCode.EmailDoesNotExists, "Email Doesn't Exists" },
            { StatusCode.InvalidUser, "Invalid User." },
            { StatusCode.InvalidOtp, "Invalid Otp." },
           
            { StatusCode.EmailNotFound, "Email not found" },
            { StatusCode.AccountNotConfirmed, "Account has not been accepted" },
            { StatusCode.AlreadyConfirmed, "Account already accepted" },
            { StatusCode.ConfirmSuccess, "Account confirmed successfully" },

            // Curriculum messages
            { StatusCode.CurriculumNotFound, "Curriculum with Id does not exist." },
            { StatusCode.CurriculumSameName, "Curriculum with the same name already exists." },

            // Chapter messages
            { StatusCode.ChapterNotFound, "Chapter with Id does not exist." },
            { StatusCode.ChapterSameName, "Chapter with the same name already exists." },

            // Chapter Unit messages
            { StatusCode.ChapterUnitNotFound, "Chapter Unit with Id does not exist." },
            { StatusCode.ChapterUnitSameName, "Chapter Unit with the same name already exists." },

            // Book messages
            { StatusCode.BookNotFound, "Book with Id does not exist." },
            { StatusCode.BookSameName, "Book with the same name already exists." },

            // Book Series messages
            { StatusCode.BookSeriesNotFound, "Book Series with Id does not exist." },
            { StatusCode.BookSeriesSameName, "Book Series with the same name already exists." },

            // Book Detail messages
            { StatusCode.BookDetailNotFound, "Book Detail with Id does not exist." }

            // Question messages
            { StatusCode.QuestionNotFound, "Question with this Id does not exist." },
            { StatusCode.QuestionSameContent, "This question already exists." },

            // Answer messages
            { StatusCode.AnswerNotFound, "Answer with this Id does not exist." },
            { StatusCode.AnswerSameContent, "This answer already exists." },

            // Exam messages
            { StatusCode.ExamNotFound, "Exam with this Id does not exist." },
            { StatusCode.ExamSameTitle, "Exam with the same title already exists." },

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
