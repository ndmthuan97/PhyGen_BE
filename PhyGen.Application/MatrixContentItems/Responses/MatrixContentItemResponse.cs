using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.MatrixContentItems.Responses
{
    public class MatrixContentItemResponse
    {
        public Guid Id { get; set; }
        public Guid MatrixId { get; set; }
        public Guid ContentItemId { get; set; }
    }
}
