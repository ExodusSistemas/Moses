using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace Moses.Web
{
    public interface IUserContext
    {
        HttpContext CurrentContext { get; }
        HttpSessionState Session { get; }
    }

    internal class UserManager : IUserContext
    {
        public HttpContext CurrentContext { get; set; }
        public HttpSessionState Session { get; set; }
    }
}
