using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Moses.Services.Email;

namespace Moses.Configuration
{
    public class MosesServiceOptions : IServiceOptions
    {
        public Func<SmtpClient> MailClientSetup { get; set; }
        public Action<ILoggingBuilder> Log { get; set; }
        public MosesAppConfiguration AppConfiguration { get; set; } = new MosesAppConfiguration();
    }

    public class MosesAppConfiguration
    {
        public MosesAppConfigurationEmailTemplates Emails { get; set; } = new MosesAppConfigurationEmailTemplates();
        public string AppName { get; set; }
        public string Title { get; set; }
        public string HomeUrl { get; set; }
    }

    
}
