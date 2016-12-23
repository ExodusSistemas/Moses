using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Moses.Web
{
    public interface IJsonService
    {
        string Serialize(object item);
        T Deserialize<T>(Stream stream);
        T Deserialize<T>(string stream);
        object Deserialize(Type type, Stream stream);
        object GetSettings();
        object DeserializeExpandoObjectFromStream(Stream stream);
        bool IsJsonStructure(Type type);
        string SerializeObject(object value);
    }


}
