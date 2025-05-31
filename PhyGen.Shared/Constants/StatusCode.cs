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
        ModelInvalid = 2001,
        UserAuthenticationFailed = 2010,
        EmailAlreadyExists = 2011,
        RegisterFailed = 2004,
        LoginFailed = 2006,
        UserNotFound = 2008,
        InvalidPassword = 2009,
        EmailDoesNotExists = 2005,
        InvalidUser = 2007,
        InvalidOtp = 2012,

        // Curriculum error
        CurriculumSameName = 2002,
        CurriculumNotFound = 2003,
    }
}
