using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Questions.Interfaces
{
    public interface IImageStorage
    {
        Task<string> SaveAsync(byte[] data, string contentType, string fileNameHint = null);
    }
}
