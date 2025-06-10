using PhyGen.Application.Authentication.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.Authentication.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequest request);
    }
}
