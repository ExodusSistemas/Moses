using System;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Moses.Data.Monitoring;
using Moses.Extensions;


namespace Moses.Web
{
    public static class ApplicationMail
    {
        public static void SendMail(this MailMessage message, NetworkCredential credential)
        {
            try
            {
                //Smtp Mail server of Gmail is "smpt.gmail.com" and it uses port no. 587
                //For different server like yahoo this details changes and you can
                //get it from respective server.
                System.Net.Mail.SmtpClient mailClient = Moses.Web.Configuration.GetMailClient();

                //Enable SSL
                mailClient.EnableSsl = true;
                mailClient.UseDefaultCredentials = false;
                mailClient.Credentials = credential;
                mailClient.Send(message);
            }
            catch { }
        }

        public static void SendMail(this MailMessage message)
        {
            try
            {
                //Smtp Mail server of Gmail is "smpt.gmail.com" and it uses port no. 587
                //For different server like yahoo this details changes and you can
                //get it from respective server.
                System.Net.Mail.SmtpClient mailClient = Moses.Web.Configuration.GetMailClient();

                mailClient.SendCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        System.Diagnostics.Trace.TraceError(e.Error.ToString());
                    }
                };

                mailClient.SendAsync(message, null);
            }
            catch { }
        }


        public static MailMessage CreateMail(string emailFrom, string displayNameFrom, string contractName, string titulo, string emailBody, string to, string replyTo = null)
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


        public static MailMessage CreateSystemMail(string titulo, string emailBody, string to, string replyTo = null)
        {
            //Retorna todos os clientes 
            try
            {
                MailMessage message = new MailMessage();
                message.To.Add(to);

                message.From = new MailAddress(Configuration.ApplicationConfiguration.SystemEmail, Configuration.ApplicationConfiguration.SystemEmailSenderName);
                message.Subject = Configuration.ApplicationConfiguration.SystemEmailSenderNotificationTag + titulo;
                message.IsBodyHtml = true;
                message.Body = emailBody;

                if (replyTo != null)
                {
                    message.ReplyToList.Add(new MailAddress(replyTo));
                    message.ReplyToList.Add(new MailAddress(Configuration.ApplicationConfiguration.SupportEmail));
                }

                return message;
            }
            catch { return null; }

        }

        public static MailMessage SendMailToChangePassword(string titulo, string emailBody, string to)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.To.Add(to);

                message.From = new MailAddress(Configuration.ApplicationConfiguration.SystemEmail, Configuration.ApplicationConfiguration.SystemEmailSenderName);
                message.Subject = Configuration.ApplicationConfiguration.SystemEmailChangePasswordTag + titulo;
                message.IsBodyHtml = true;
                message.Body = emailBody;

                return message;
            }
            catch
            {
                return null;
            }
        }

        //public static MailMessage CreateBillingMail(string titulo, string emailBody, string to, params Attachment[] a)
        //{
        //    //Retorna todos os clientes 
        //    try
        //    {
        //        MailMessage message = new MailMessage();
        //        //message.To.Add("cobranca@exodus.eti.br");
        //        message.To.Add(to);
        //        message.From = new MailAddress("cobranca@exodus.eti.br", "Cobrança Exodus");
        //        message.Subject = titulo;
        //        message.IsBodyHtml = true;
        //        message.Body = emailBody;
        //        message.ReplyTo = new MailAddress("cobranca@exodus.eti.br");

        //        foreach (var at in a)
        //            message.Attachments.Add(at);

        //        return message;
        //    }
        //    catch { return null; }

        //}

        public static MailMessage CreateErrorMail(string contextUrl, Exception exception, string userName, string contractName, string referer = "")
        {
            string emailTo = Configuration.BugTrackingMail;
            try
            {
                MailMessage message = new MailMessage();
                message.To.Add(emailTo);
                message.From = new MailAddress(Configuration.ApplicationConfiguration.SystemEmail, Configuration.ApplicationConfiguration.SystemEmailSenderName);

                message.Subject = Configuration.ApplicationConfiguration.SystemEmailErrorTag +  exception.Message;
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

        public static void SendMail(this MailMessage message, IMailClient client, NetworkCredential network = null, bool async = true)
        {
            if (client == null)
            {
                SendMail(message);
                return;
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
                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

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
