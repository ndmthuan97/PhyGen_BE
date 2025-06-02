using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Shared.Constants
{
    public enum StatusCode
    {
        // Success code 1xxx
        RequestProcessedSuccessfully = 1000,
        RegisterSuccess = 1001,
        LoginSuccess = 1002,
        ChangedPasswordSuccess = 1003,
        OtpSendSuccess = 1004,

        // Error code 2xxx
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
        EmailNotFound = 2018,
        AccountNotConfirmed = 2019,
        AlreadyConfirmed = 2020,
        ConfirmSuccess = 2021,

        // Curriculum error
        CurriculumSameName = 2010,
        CurriculumNotFound = 2011,

        // Chapter error
        ChapterNotFound = 2012,
        ChapterSameName = 2013,

        // Book error
        BookNotFound = 2014,
        BookSameName = 2015,

        // Chapter Unit error
        ChapterUnitNotFound = 2016,
        ChapterUnitSameName = 2017,

        // Book Series error
        BookSeriesNotFound = 2022,
        BookSeriesSameName = 2023,

        // Book Detail error
        BookDetailNotFound = 2030,
        
        // Question error
        QuestionNotFound = 2024,
        QuestionSameContent = 2025,
        
        // Answer error
        AnswerNotFound = 2026,
        AnswerSameContent = 2027,

        // Exam error
        ExamNotFound = 2028,
        ExamSameTitle = 2029,
    }
}
