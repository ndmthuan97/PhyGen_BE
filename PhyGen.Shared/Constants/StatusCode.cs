using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Shared.Constants
{
    public enum StatusCode
    {
        // Success code (1000–1099)
        RequestProcessedSuccessfully = 1000,
        RegisterSuccess = 1001,
        LoginSuccess = 1002,
        ChangedPasswordSuccess = 1003,
        OtpSendSuccess = 1004,

        // General errors (2000–2015)
        ModelInvalid = 2000,

        // User-related errors (2016–2049)
        UserAuthenticationFailed = 2003,
        UserNotFound = 2004,

        // Curriculum errors (2050–2069)
        CurriculumSameName = 2050,
        CurriculumNotFound = 2051,

        // Chapter errors (2070–2089)
        ChapterNotFound = 2070,
        ChapterSameName = 2071,

        // Subject errors (2090–2109)
        SubjectNotFound = 2090,
        SubjectSameName = 2091,

        // Subject Curriculum errors (2110–2129)
        SubjectCurriculumNotFound = 2110,

        // Chapter Unit errors (2130–2149)
        ChapterUnitNotFound = 2130,
        ChapterUnitSameName = 2131,

        // Authentication errors (2150–2200)
        RegisterFailed = 2151,
        LoginFailed = 2152,
        EmailAlreadyExists = 2155,
        EmailDoesNotExists = 2156,
        InvalidUser = 2157,
        InvalidPassword = 2158,
        InvalidOtp = 2159,
        EmailNotFound = 2160,
        AccountNotConfirmed = 2161,
        AlreadyConfirmed = 2162,
        ConfirmSuccess = 2163,
        PasswordMismatch = 2164,
        InvalidPasswordFormat= 2165,
        InvalidGoogleToken = 2166,
        InvalidToken = 2167,
        MustLoginWithEmailPassword = 2168,
        MustLoginWithGoogle = 2169,
        
        // Exam errors (2150–2169)
        ExamNotFound = 2150,
    }
}
