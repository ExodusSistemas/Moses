using Newtonsoft.Json;
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
            Settings = new JsonSerializerSettings
            {
                MaxDepth = 1,
                Formatting = Newtonsoft.Json.Formatting.None,
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
                ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Reuse,
                MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None, //by olavo
                ReferenceLoopHandling = ReferenceLoopHandling.Error,
                ContractResolver = new ExcludeEntityKeyContractResolver()
            };
        }

        public JsonSerializerSettings Settings { get; private set; }

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

            var scriptSerializer = JsonSerializer.Create(this.Settings);

            using (var sw = new StringWriter())
            {
                scriptSerializer.Serialize(sw, this.Data);
                response.Write(sw.ToString());
            }
        }
    }
}

