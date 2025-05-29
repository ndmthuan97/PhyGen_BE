using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Specs
{
    // Base class containing standard pagination parameters used for Specification.
    public abstract class BaseSpecParam
    {
        public const int MaxPageSize = AppConstant.MAX_PAGE_SIZE;

        public int PageIndex { get; set; } = 1;

        private int _pageSize = AppConstant.DEFAULT_PAGE_SIZE;

        // Page size (number of records per page). 
        // If the value is greater than MaxPageSize, it is automatically limited to MaxPageSize.
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}
