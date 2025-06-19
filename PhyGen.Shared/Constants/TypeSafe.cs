using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Shared.Constants
{
    public enum Role
    {
        Admin,
        User
    }

    public enum DifficultyLevel
    {
        NhậnBiết,
        ThôngHiểu,
        VậnDụng
    }

    public enum QuestionType
    {
        MultipleChoice,
        TrueFalse,
        ShortAnswer,
        Essay
    }
}
