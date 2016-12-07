using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Moses.Web.Mvc;
using System.IO;
using System.Web.Mvc;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Converters;
using Moses.Serialization;
using System.Reflection;
using Moses.Web.Formatters;

namespace Moses.Web.Formatters
{
    public class JsonNet2Formatter : MediaTypeFormatter
    {
        public JsonNet2Formatter() :base()
        {
            SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
        }

        public JsonSerializerSettings Settings = new JsonSerializerSettings()
        {
            MaxDepth = 2,
            Formatting = Newtonsoft.Json.Formatting.None,
            NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
            ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Reuse,
            MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.None, //by olavo
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new ExcludeEF2Association()
        };

        public override bool CanWriteType(Type type)
        {
            // don't serialize JsonValue structure use default for that
            if (type == typeof(JValue) || type == typeof(JObject) || type == typeof(JArray))
                return false;

            return true;
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override System.Threading.Tasks.Task<object> ReadFromStreamAsync(Type type,
                                                            Stream stream,
                                                            HttpContent content,
                                                            IFormatterLogger formatterLogger)
        {
            var task = Task<object>.Factory.StartNew(() =>
            {


                var sr = new StreamReader(stream);
                var jreader = new JsonTextReader(sr);

                var ser = JsonSerializer.Create(Settings);
                ser.Converters.Add(new IsoDateTimeConverter());

                object val = ser.Deserialize(jreader, type);
                return val;
            });

            return task;
        }

        public override Task WriteToStreamAsync(Type type, object value,
                                                Stream stream,
                                                HttpContent content,
                                                TransportContext transportContext)
        {
            var task = Task.Factory.StartNew(() =>
            {


                string json = JsonConvert.SerializeObject(value, Formatting.Indented,
                                                          new JsonConverter[1] { new IsoDateTimeConverter() });

                byte[] buf = System.Text.Encoding.Default.GetBytes(json);
                stream.Write(buf, 0, buf.Length);
                stream.Flush();
            });

            return task;
        }


    }
}


namespace Moses.Serialization
{
    public class JsonNetResult : JsonResult
    {
        public JsonNetResult()
        {
            Settings = System.Web.Http.GlobalConfiguration.Configuration.Formatters.OfType<JsonNet2Formatter>().FirstOrDefault().Settings;
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

    public class ExcludeEF2Association : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);
            return properties.FilterEntities().Where(p => !p.PropertyType.IsGenericType || (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() != typeof(HashSet<>))).ToList();
        }

    }

    public static class ExcludeEntityKeyHelper
    {

        /// <summary>
        /// Exclui da lista todas as variáveis que representam entidades associadas
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static IList<JsonProperty> FilterEntities(this IList<JsonProperty> properties)
        {
            List<JsonProperty> output = new List<JsonProperty>(properties);
            foreach (var property in properties)
            {
                var memberInfos = property.DeclaringType.GetMember(property.PropertyName);
                if (memberInfos.Length == 1)
                {
                    AssociationAttribute attr = memberInfos[0].GetCustomAttributes(typeof(AssociationAttribute), false).FirstOrDefault() as AssociationAttribute;

                    if (attr != null)
                        if (attr.IsForeignKey == true)
                            output.Remove(property);
                }

            }

            return output;
        }

    }
}