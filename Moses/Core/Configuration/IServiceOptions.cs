using System;
using System.Net.Mail;
using Microsoft.Extensions.Logging;

namespace Moses.Configuration
{
    public interface IServiceOptions
    {
        MosesAppConfiguration AppConfiguration { get; set; }
        Action<ILoggingBuilder> Log { get; set; }
        Func<SmtpClient> MailClientSetup { get; set; }
    }
}