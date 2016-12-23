using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Dynamic;
using System.Linq;

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
            MaxDepth = 4,
            Formatting = Newtonsoft.Json.Formatting.None,
            NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
            MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.None, //by olavo
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new ExcludeEntityKeyContractResolver()
        };

        public object GetSettings()
        {
            return this.Settings;
        }

        public object DeserializeExpandoObjectFromStream(Stream stream)
        {
            StreamReader streamReader = new StreamReader(stream);
            // convert stream reader to a JSON Text Reader
            JsonTextReader jsonReader = new JsonTextReader(streamReader);
            // tell JSON to read
            if (!jsonReader.Read())
                return null;

            // make a new Json serializer
            JsonSerializer jsonSerializer = JsonSerializer.Create(Settings);
            // add the dyamic object converter to our serializer
            jsonSerializer.Converters.Add(new ExpandoObjectConverter());

            // use JSON.NET to deserialize object to a dynamic (expando) object
            Object jsonObject;
            // if we start with a "[", treat this as an array
            if (jsonReader.TokenType == JsonToken.StartArray)
                jsonObject = jsonSerializer.Deserialize<List<ExpandoObject>>(jsonReader);
            else
                jsonObject = jsonSerializer.Deserialize<ExpandoObject>(jsonReader);

            return jsonObject;
        }

        public bool IsJsonStructure(Type type)
        {
            return type == typeof(JValue) || type == typeof(JObject) || type == typeof(JArray);
        }

        public string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value, Formatting.Indented,
                                                          new JsonConverter[1] { new IsoDateTimeConverter() });
        }
    }

    public class ExcludeEntityKeyContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);
            return properties.FilterEntities().Where(p => !p.PropertyType.IsGenericType || (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() != typeof(Moses.Data.EntitySet<>))).ToList();
        }
    }

    public static class ExcludeEntityKeyHelper{
        
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
                    //AssociationAttribute attr = memberInfos[0].GetCustomAttributes(typeof(AssociationAttribute), false).FirstOrDefault() as AssociationAttribute;
                    //
                    //if (attr != null)
                    //    if (attr.IsForeignKey == true)
                    //        output.Remove(property);
                }
                
            }

            return output;
        }

    }
}
