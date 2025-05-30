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

        // Curriculum error
        CurriculumSameName = 2002,
        CurriculumNotFound = 2003,
    }
}
