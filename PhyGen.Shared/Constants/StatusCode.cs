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

        // General errors (2000–2049)
        ModelInvalid = 2000,
        RegisterFailed = 2001,
        LoginFailed = 2002,
        UserAuthenticationFailed = 2003,
        UserNotFound = 2004,
        EmailAlreadyExists = 2005,
        EmailDoesNotExists = 2006,
        InvalidUser = 2007,
        InvalidPassword = 2008,
        InvalidOtp = 2009,
        EmailNotFound = 2010,
        AccountNotConfirmed = 2011,
        AlreadyConfirmed = 2012,
        ConfirmSuccess = 2013,
        PasswordMismatch = 2014,

        // Curriculum errors (2050–2069)
        CurriculumSameName = 2050,
        CurriculumNotFound = 2051,

        // Chapter errors (2070–2089)
        ChapterNotFound = 2070,
        ChapterSameName = 2071,

        // Book errors (2090–2109)
        BookNotFound = 2090,
        BookSameName = 2091,

        // Chapter Unit errors (2110–2129)
        ChapterUnitNotFound = 2110,
        ChapterUnitSameName = 2111,

        // Book Series errors (2130–2149)
        BookSeriesNotFound = 2130,
        BookSeriesSameName = 2131,

        // Book Detail errors (2150–2169)
        BookDetailNotFound = 2150,

        // Question errors (2170–2189)
        QuestionNotFound = 2170,
        QuestionSameContent = 2171,

        // Answer errors (2190–2209)
        AnswerNotFound = 2190,
        AnswerSameContent = 2191,

        // Exam errors (2210–2229)
        ExamNotFound = 2210,
        ExamSameTitle = 2211,

        // Matrix errors (2230–2249)
        MatrixNotFound = 2230,
        MatrixSameName = 2231,

        // Matrix Detail errors (2250–2269)
        MatrixDetailNotFound = 2250,
    }
}
