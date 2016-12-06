using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moses.Extensions
{
    public class EnumSelector
    {
        public int Id { get; set; }
        public string value { get; set; }
    }

    public static class EnumHelper
    {
        public static Dictionary<Type, List<EnumSelector>> EnumDatabaseCache = new Dictionary<Type, List<EnumSelector>>();

        public static IEnumerable<string> GetDescriptions(this Type enumType)
        {
            return enumType.GetEnumSelector().Select(q => q.value);
        }

        public static List<EnumSelector> GetEnumSelector(this Type enumType)
        {
            if (!EnumDatabaseCache.ContainsKey(enumType))
            {
                var fields = enumType.GetFields();

                List<EnumSelector> descs = new List<EnumSelector>();
                foreach (var field in fields)
                {
                    if (!field.IsStatic) continue;
                    var attributes = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                    string description = attributes.Length == 0 ? field.Name.ToString() : ((System.ComponentModel.DescriptionAttribute)attributes[0]).Description;
                    descs.Add(new EnumSelector() { value = description, Id = (int)field.GetValue(null) });
                }

                EnumDatabaseCache.Add(enumType, descs);
            }

            return EnumDatabaseCache[enumType];
        }

    }
}
