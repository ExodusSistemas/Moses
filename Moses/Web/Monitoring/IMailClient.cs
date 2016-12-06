using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moses.Data.Monitoring
{
    public interface IMailClient
    {
        string DisplayEmail { get; set; }

        string DisplayName { get; set; }

        string ReplyTo { get; set; }

        string CcList { get; set; }

        string CcoList { get; set; }

        string Host { get; set; }

        int? Port { get; set; }

        bool? EnableSsl { get; set; }

        string UserName { get; set; }

        string Password { get; set; }
    }
}
