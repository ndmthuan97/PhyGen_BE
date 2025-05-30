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

        // Error code 2xxx
        ModelInvalid = 2001,
        UserAuthenticationFailed = 2002,
        EmailAlreadyExists = 2003,
        RegisterFailed = 2004,
        RegisterSuccess = 2005,
        LoginFailed = 2006,
        LoginSuccess = 2007,
        UserNotFound = 2008,
        InvalidPassword = 2009,

        // Curriculum error
        CurriculumSameName = 2002,
        CurriculumNotFound = 2003,
    }
}
