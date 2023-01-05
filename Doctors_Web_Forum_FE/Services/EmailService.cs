using Doctors_Web_Forum_FE.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Doctors_Web_Forum_FE.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;
        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        //Send Email With Token
        public async Task SendEmailAsync(String ToEmail,String Token)
        {
            var Link = "http://localhost:18039/login/active-accounts/" + ToEmail+"/"+ Token;
            string FilePath = Directory.GetCurrentDirectory() + "\\Util\\email.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            MailText = MailText.Replace("[link]", Link);
            str.Close();
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(ToEmail));
            email.Subject = "Complete Registration With Health Talk";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }


    }
}
