using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moses.Extensions;
using System.Web.Mvc;

namespace Moses.Extensions
{
    public static class PropertyValueHolderHelper
    {
        public static string SerializeToJson(this Dictionary<string, object> propertyDictionary)
        {
            if (propertyDictionary.Count == 0) return "";
            return propertyDictionary.ToJSon();
        }

        public static void RefreshProperties(this Dictionary<string, object> propertyDictionary, string propertyValuesString)
        {
            propertyDictionary.Clear();
            propertyDictionary = propertyValuesString.JsonToDictionary();
        }

        public static IEnumerable<SelectListItem> GetListFromEnum(this Type enumType)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            // get the names from the enumeration
            string[] names = Enum.GetNames(enumType);
            // get the values from the enumeration
            Array values = Enum.GetValues(enumType);

            for (int i = 0; i < names.Length; i++)
            {
                // note the cast to integer here is important
                // otherwise we'll just get the enum string back again
                list.Add(new SelectListItem() { Value = values.GetValue(i).ToString(), Text = names[i] });
            }

            return list;
        }
    }
}
