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

        // Exam errors (2150–2169)
        ExamNotFound = 2150,
    }
}
