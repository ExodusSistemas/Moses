using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moses.Configuration;
using Moses.Net;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Moses.Extensions;

namespace Moses.Services
{
    public class EmailService : IEmailService
    {
        private readonly MosesServiceOptions options;
        private readonly ILogger _log;

        public async Task SendAsync(MailMessage message)
        {
            
            if (options.MailClientSetup != null)
            {
                SmtpClient mailClient = options.MailClientSetup();
                _log.LogInformation($"Sending e-mail to: {message.To}");
                await mailClient.SendMailAsync(message);
            }
        }

        public MailMessage CreateMail(string emailFrom, string displayNameFrom, string contractName, string titulo, string emailBody, string to, string replyTo = null)
        {
            //Retorna todos os clientes 
            try
            {
                MailMessage message = new MailMessage();
                message.To.Add(to);

                message.From = new MailAddress(emailFrom, displayNameFrom);
                message.Subject = @"[" + contractName + "] " + titulo;
                message.IsBodyHtml = true;
                message.Body = emailBody;

                if (replyTo != null)
                {
                    message.ReplyToList.Add(new MailAddress(replyTo));
                }

                return message;
            }
            catch { return null; }
        }

        public MailMessage CreateSystemMail(string titulo, string emailBody, string to, string replyTo = null)
        {
            //Retorna todos os clientes 
            try
            {
                MailMessage message = new MailMessage();
                message.To.Add(to);

                message.From = new MailAddress(options.AppConfiguration.Emails.SystemEmail.Address, options.AppConfiguration.Emails.SystemEmail.DisplayName);
                message.Subject = options.AppConfiguration.Emails.SystemEmail.SubjectTag + titulo;
                message.IsBodyHtml = true;
                message.Body = emailBody;

                if (replyTo != null)
                {
                    message.ReplyToList.Add(new MailAddress(replyTo));
                    message.ReplyToList.Add(new MailAddress(options.AppConfiguration.Emails.SupportEmail.Address));
                }

                return message;
            }
            catch { return null; }

        }

        public MailMessage SendMailToChangePassword(string title, string emailBody, string to)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.To.Add(to);

                message.From = new MailAddress(options.AppConfiguration.Emails.ChangePasswordEmail.Address, options.AppConfiguration.Emails.ChangePasswordEmail.DisplayName);
                message.Subject = options.AppConfiguration.Emails.ChangePasswordEmail.SubjectTag + title;
                message.IsBodyHtml = true;
                message.Body = emailBody;

                return message;
            }
            catch
            {
                return null;
            }
        }

        public MailMessage CreateErrorMail(string contextUrl, Exception exception, string userName, string contractName, string referer = "")
        {
            string emailTo = options.AppConfiguration.Emails.BugTrackEmail.Address;
            try
            {
                MailMessage message = new MailMessage();
                message.To.Add(emailTo);
                message.From = new MailAddress(options.AppConfiguration.Emails.ErrorEmail.Address, options.AppConfiguration.Emails.ErrorEmail.DisplayName);

                message.Subject = options.AppConfiguration.Emails.ErrorEmail.SubjectTag + exception.Message;
                message.IsBodyHtml = true;
                message.Body =
                             "<br>Contract Name: " + contractName +
                             "<br>Referer: " + referer +
                             "<br>User: " + userName +
                             "<br>Offending URL: " + contextUrl +
                             "<br>Source: " + exception.Source +
                             "<br>Message: " + exception.Message +
                             "<br>Inner Exception: " + exception.InnerException +
                             "<br>Stack trace: " + exception.StackTrace;

                message.ReplyToList.Add(new MailAddress(emailTo));

                return message;
            }
            catch { return null; }

        }

        public async void SendMail(MailMessage message, IMailClient client, NetworkCredential network = null, bool async = true)
        {
            if (client == null)
            {
                await SendAsync(message);
            }
            else
            {
                message.From = new MailAddress(client.DisplayEmail, client.DisplayName);
                try
                {
                    if (!string.IsNullOrEmpty(client.ReplyTo))
                        message.ReplyToList.Add(client.ReplyTo);
                    if (!string.IsNullOrEmpty(client.CcList))
                        message.CC.Add(client.CcList);
                    if (!string.IsNullOrEmpty(client.CcoList))
                        message.Bcc.Add(client.CcoList);

                }
                catch { }
            }

            try
            {
                //Smtp Mail server of Gmail is "smpt.gmail.com" and it uses port no. 587
                //For different server like yahoo this details changes and you can
                //get it from respective server.
                System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient(client.Host, client.Port ?? 25);

                //Enable SSL
                mailClient.EnableSsl = client.EnableSsl ?? false;
                mailClient.UseDefaultCredentials = false;

                //todo: parametrizar isso
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                if (network == null)
                {
                    network = new NetworkCredential(client.UserName, client.Password.Decypher());
                }
                mailClient.Credentials = network;

                mailClient.SendCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        System.Diagnostics.Trace.TraceError(e.Error.ToString());
                    }
                };

                if (async)
                    mailClient.SendAsync(message, null);
                else
                    mailClient.Send(message);
            }
            catch (SmtpException smex)
            {
                throw new MosesApplicationException("Para o envio dos e-mails, é necessária uma conexão segura. Não foi possível autenticar a conta de email utilizada para os envios:" + smex.Message, smex);
            }
            catch (Exception ex)
            {
                throw new MosesApplicationException("Não foi possível enviar o email:" + ex.Message, ex);
            }
        }


    }
}
