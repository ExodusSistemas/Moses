using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Moses.Web.Services
{
    public class JsonService : IJsonService
    {
        public object Deserialize(Type type, Stream stream)
        {
            var sr = new StreamReader(stream);
            var jreader = new JsonTextReader(sr);

            var ser = JsonSerializer.Create(Settings);
            ser.Converters.Add(new IsoDateTimeConverter());

            return ser.Deserialize(jreader, type);
        }

        public T Deserialize<T>(Stream stream)
        {
            var sr = new StreamReader(stream);
            var jreader = new JsonTextReader(sr);

            var ser = JsonSerializer.Create(Settings);
            ser.Converters.Add(new IsoDateTimeConverter());

            return ser.Deserialize<T>(jreader);
        }

        public T Deserialize<T>(string serializedString)
        {
            return Deserialize<T>(GenerateStreamFromString(serializedString));
        }

        private Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public string Serialize(object item)
        {
            var ser = JsonSerializer.Create(Settings);
            StringWriter writer = new StringWriter();
            ser.Serialize(writer,item);
            return writer.ToString();
        }

        public JsonSerializerSettings Settings { get; set; } = new JsonSerializerSettings()
        {
            MaxDepth = 2,
            Formatting = Newtonsoft.Json.Formatting.None,
            NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
            ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace,
            MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.None, //by olavo
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
    }
}
