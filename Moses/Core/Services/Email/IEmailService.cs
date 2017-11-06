using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Moses.Net;

namespace Moses.Services
{
    public interface IEmailService
    {
        MailMessage CreateErrorMail(string contextUrl, Exception exception, string userName, string contractName, string referer = "");
        MailMessage CreateMail(string emailFrom, string displayNameFrom, string contractName, string titulo, string emailBody, string to, string replyTo = null);
        MailMessage CreateSystemMail(string titulo, string emailBody, string to, string replyTo = null);
        Task SendAsync(MailMessage message);
        void SendMail(MailMessage message, IMailClient client, NetworkCredential network = null, bool async = true);
        MailMessage SendMailToChangePassword(string title, string emailBody, string to);
    }
}