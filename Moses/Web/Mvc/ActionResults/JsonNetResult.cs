using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Moses.Web.Extensions;

namespace Moses.Web.Mvc
{
    public class JsonNetResult : JsonResult
    {
        public JsonNetResult()
        {
            
        }

        /// <summary>
        /// este eh um x
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("JSON GET is not allowed");

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;

            if (this.ContentEncoding != null)
                response.ContentEncoding = this.ContentEncoding;
            if (this.Data == null)
                return;

            response.Write(Configuration.Json.Serialize(this.Data));
        }
    }
}

