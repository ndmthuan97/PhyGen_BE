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

        // Authentication success
        RegisterSuccess = 2005,
        LoginSuccess = 2007,

        // Error code 2xxx
        ModelInvalid = 2001,

        // Authentication error
        UserAuthenticationFailed = 2002,
        EmailAlreadyExists = 2003,
        RegisterFailed = 2004,
        LoginFailed = 2006,
        UserNotFound = 2008,
        InvalidPassword = 2009,

        // Curriculum error
        CurriculumSameName = 2010,
        CurriculumNotFound = 2011,

        // Chapter error
        ChapterNotFound = 2012,
        ChapterSameName = 2013,

        // Book error
        BookNotFound = 2014,
        BookSameName = 2015,
    }
}
