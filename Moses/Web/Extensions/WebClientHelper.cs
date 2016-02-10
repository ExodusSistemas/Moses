using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.IO;
using Moses.Web.Extensions;
using Newtonsoft.Json;


namespace Moses.Extensions
{
    public static class WebClientHelper
    {
        private static string _aspa = "\'";

        public static string ToJavascriptArray(this string[] array)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var a in array)
            {
                builder.Append(_aspa);
                builder.Append(a.Replace("\'","\\'"));
                builder.Append(_aspa);
                builder.Append(",");
            }

            if (array.Length < 1) return "null";

            return string.Format("[ {0} ]", builder.Remove(builder.Length - 1, 1));
        }

        public static string SerializeToJavascript(this object target)
        {
            return JsonConvert.SerializeObject(target, Formatting.None, new JsonSerializerSettings()
            {
                MaxDepth = 1,
                Formatting = Newtonsoft.Json.Formatting.None,
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
                ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Reuse,
                MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None, //by olavo
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new ExcludeEntityKeyContractResolver()
            }); 
        }

        //alias
        public static string ToJSon(this object target)
        {
            return SerializeToJavascript(target);
        }

        //alias
        public static string FromJSon(this string target)
        {
            return JsonConvert.DeserializeObject<string>(target);
        }

        public static T FromJSon<T>(this string target)
        {
            return JsonConvert.DeserializeObject<T>(target);
        }

        public static Dictionary<string,object> JsonToDictionary(this string target)
        {
            if (string.IsNullOrEmpty(target)) return new Dictionary<string, object>();

            var output = JsonConvert.DeserializeObject<Dictionary<string, object>>(target);

            if (output == null) return new Dictionary<string, object>();

            //correção de um bug do ASP.NET (a hora está sendo reconvertida em UTC, e isso gera uma diferença em horas 
            var j = output.Count;
            for( int i = 0; i < j ; i++)
            {
                var el = output.ElementAt(i);
                if (el.Value is DateTime)
                {
                    var date =(DateTime)el.Value;

                    DateTime utc = DateTime.UtcNow;
                    output[el.Key] = (object)(date).AddTicks(utc.ToLocalTime().Ticks - utc.Ticks);
                }
            }
            

            return output;
        }

        public static string JsonToCsv(this string target)
        {
            throw new NotImplementedException();
        }

        public static string ToCsvString<T>(this IEnumerable<T> target , string prefix = "", string sufix = "")
        {
            StringBuilder bdr = new StringBuilder();

            foreach (var t in target)
            {
                bdr.AppendFormat(",{1}{0}{2}", t.ToString() , prefix, sufix );
            }

            if ( bdr.Length > 1){
                return bdr.ToString(1,bdr.Length-1);
            }

            return "";
        }

        
    }
}
