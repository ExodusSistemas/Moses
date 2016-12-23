using System;
using System.Threading.Tasks;
using System.IO;
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

        public override bool CanWriteType(Type type)
        {
            // don't serialize JsonValue structure use default for that
            if (Configuration.Json.IsJsonStructure(type))
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
                object val = Configuration.Json.Deserialize(type, stream);
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


                string json = Configuration.Json.SerializeObject(value);

                byte[] buf = System.Text.Encoding.Default.GetBytes(json);
                stream.Write(buf, 0, buf.Length);
                stream.Flush();
            });

            return task;
        }


    }


}
