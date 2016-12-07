using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Moses.Web.Manager
{
    public class ManagerHandlerFactory : System.Web.UI.PageHandlerFactory
    {
        public override IHttpHandler GetHandler(HttpContext context, string requestType, string virtualPath, string path)
        {
            return base.GetHandler(context, requestType,
                context.Request.ApplicationPath + "/Manager/Default.aspx", path);
        }

        public override void ReleaseHandler(IHttpHandler handler)
        {
            base.ReleaseHandler(handler);
        }
    }
}
