using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http;

namespace Moses.Web.Formatters
{

    public class JsonNetFormatter : MediaTypeFormatter
    {
        public JsonNetFormatter()
        {
            SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
        }

        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings()
        {
            MaxDepth = 2,
            Formatting = Newtonsoft.Json.Formatting.None,
            NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
            ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace,
            MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.None, //by olavo
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
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
