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

        // Subject errors (2070–2089)
        SubjectSameName = 2070,
        SubjectNotFound = 2071,

        // SubjectBook errors (2090–2109)
        SubjectBookSameName = 2090,
        SubjectBookNotFound = 2091,

        // Chapter errors (2110–2129)
        ChapterSameName = 2110,
        ChapterNotFound = 2111,

        // Topic errors (2130–2149)
        TopicSameName = 2130,
        TopicNotFound = 2131,
    }
}
