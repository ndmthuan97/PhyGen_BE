using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Status
{
    public static class CheckCanChangeStatus
    {
        public static bool CanChangeStatus(StatusQEM current, StatusQEM target)
        {
            return current switch
            {
                StatusQEM.Draft => target == StatusQEM.Approved || target == StatusQEM.Removed,
                StatusQEM.AIDraft => target == StatusQEM.Approved || target == StatusQEM.Removed,
                StatusQEM.Approved => target == StatusQEM.Removed,
                StatusQEM.Removed => target == StatusQEM.Draft || target == StatusQEM.AIDraft,
                _ => false
            };
        }
    }
}
