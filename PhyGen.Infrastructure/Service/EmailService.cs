using Microsoft.Extensions.Options;
using PhyGen.Application.Authentication.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PhyGen.Application.Authentication.Models.Requests;
using AutoMapper.Internal;
using MailKit.Security;
using MimeKit;

namespace PhyGen.Infrastructure.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            emailSettings = options.Value;
        }

        public async Task SendEmailAsync(EmailRequest request)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(emailSettings.Email);
            email.To.Add(MailboxAddress.Parse(request.Email));
            email.Subject = request.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = request.Emailbody;
            email.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient(); // Use MailKit's SmtpClient
            await smtp.ConnectAsync(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls); // Use ConnectAsync
            await smtp.AuthenticateAsync(emailSettings.Email, emailSettings.Password); // Use AuthenticateAsync
            await smtp.SendAsync(email); // Use SendAsync
            await smtp.DisconnectAsync(true); // Use DisconnectAsync
        }
    }
}