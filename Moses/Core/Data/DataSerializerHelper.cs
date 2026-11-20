using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Moses.Extensions
{
    public static class DataSerializerHelper
    {
        private static string _aspa = "\'";

        public static string ToJavascriptArray(this string[] array)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var a in array)
            {
                builder.Append(_aspa);
                builder.Append(a.Replace("\'", "\\'"));
                builder.Append(_aspa);
                builder.Append(",");
            }

            if (array.Length < 1) return "null";

            return string.Format("[ {0} ]", builder.Remove(builder.Length - 1, 1));
        }

        //alias
        public static string ToJSon(this object target)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(target);
        }

        public static string FromJSon(this string target)
        {
            return FromJSon<string>(target);
        }

        //alias
        public static T FromJSon<T>(this string target) 
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(target);
        }

        public static Dictionary<string, object> JsonToDictionary(this string target)
        {
            if (string.IsNullOrEmpty(target)) return new Dictionary<string, object>();

            var settings = new JsonSerializerSettings
            {
                FloatParseHandling = FloatParseHandling.Decimal
            };

            return JsonConvert.DeserializeObject<Dictionary<string, object>>(target, settings)
                ?? new Dictionary<string, object>();
        }

        public static string JsonToCsv(this string target)
        {
            throw new NotImplementedException();
        }

        public static string ToCsvString<T>(this IEnumerable<T> target, string prefix = "", string sufix = "")
        {
            StringBuilder bdr = new StringBuilder();

            foreach (var t in target)
            {
                bdr.AppendFormat(",{1}{0}{2}", t.ToString(), prefix, sufix);
            }

            if (bdr.Length > 1)
            {
                return bdr.ToString(1, bdr.Length - 1);
            }

            return "";
        }


    }
}