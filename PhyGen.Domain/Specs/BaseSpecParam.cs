using PhyGen.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Specs
{
    public abstract class BaseSpecParam
    {
        public const int MaxPageSize = AppConstant.MAX_PAGE_SIZE;
        public int PageIndex { get; set; } = 1;
        private int _pageSize = AppConstant.DEFAULT_PAGE_SIZE;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}
