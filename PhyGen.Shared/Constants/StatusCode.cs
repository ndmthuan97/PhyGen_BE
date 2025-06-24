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
        CurriculumAlreadyExist = 2050,
        CurriculumNotFound = 2051,

        // Subject errors (2070–2089)
        SubjectSameName = 2070,
        SubjectNotFound = 2071,

        // SubjectBook errors (2090–2109)
        SubjectBookAlreadyExist = 2090,
        SubjectBookNotFound = 2091,

        // Chapter errors (2110–2129)
        ChapterAlreadyExist = 2110,
        ChapterNotFound = 2111,

        // Topic errors (2130–2149)
        TopicAlreadyExist = 2130,
        TopicNotFound = 2131,

        // ContentFlow errors (2150–2169)
        ContentFlowAlreadyExist = 2150,
        ContentFlowNotFound = 2151,

        // ContentItem errors (2170–2189)
        ContentItemAlreadyExist = 2170,
        ContentItemNotFound = 2171,

        // ExamCategory errors (2190–2209)
        ExamCategorySameName = 2190,
        ExamCategoryNotFound = 2191,

        // ExamCategoryChapter errors (2210–2229)
        ExamCategoryChapterNotFound = 2211,
        ExamCategoryChapterAlreadyExist = 2212,

        // ContentItemExamCategory errors (2230–2249)
        ContentItemExamCategoryNotFound = 2231,
        ContentItemExamCategoryAlreadyExist = 2232,

        // Authentication errors (2250–2300)
        RegisterFailed = 2250,
        LoginFailed = 2251,
        EmailAlreadyExists = 2252,
        EmailDoesNotExists = 2253,
        InvalidUser = 2254,
        InvalidPassword = 2255,
        InvalidOtp = 2256,
        EmailNotFound = 2257,
        AccountNotConfirmed = 2258,
        AlreadyConfirmed = 2259,
        ConfirmSuccess = 2260,
        PasswordMismatch = 2261,
        InvalidPasswordFormat = 2262,
        InvalidGoogleToken = 2263,
        InvalidToken = 2264,
        MustLoginWithEmailPassword = 2265,
        MustLoginWithGoogle = 2266,
        AccountLocked = 2267,

        // Notification errors (2540–2559)
        NotifcationNotFound = 2541,
        NotifcationSend = 2542,

        // Question errors (2560–2599)
        QuestionNotFound = 2560,
        QuestionAlreadyExist = 2561,
    }
}
