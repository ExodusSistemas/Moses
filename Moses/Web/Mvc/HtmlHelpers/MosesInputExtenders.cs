using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Moses.Extensions;

namespace Moses.Web.Mvc.Html
{
    using System.Web.Routing;
    using System.Web;
    using System.Linq.Expressions;

    public static class FormatFunctionSets
    {

        #region ItemFormats
        public static string DefaultEmailItemFormat
        {
            get
            {
                return "function(row, i, max) {" +
                    "    return \"\" + row.name + \"\" + \"&lt;\" + row.to + \"&gt;\";" +
                    "}";
            }
        }

        public static string DefaultKeyValueItemFormat
        {
            get
            {
                return "function(row, i, max) {" +
                    "    return \"\" + row.Key + \"\" + \"(\" + row.Value + \")\";" +
                    "}";
            }
        }

        public static string DefaultItemFormat
        {
            get
            {
                return "function(data, i, total) {" +
                    "return data[0];" +
                "}";
            }
        }

        #endregion

        #region ParseFormats

        public static string ParseDefaultEmail
        {
            get
            {
                return "function(data)  {  var rows = new Array();   for (var i = 0; i < data.length; i++) { rows[i] = { data: data[i], value: data[i].to, result: data[i].to };   }  return rows; } ";
            }
        }

        public static string ParseDefaultDictionary
        {
            get
            {
                return "function(data)  {  var rows = new Array();   for (var i = 0; i < data.length; i++) { rows[i] = { data: data[i], value: data[i].value, result: data[i].key };   }  return rows; } ";
            }
        }

        #endregion

        #region CountItemFormats

        public static string CountKeyValueItemFormat
        {
            get
            {
                return "function(row, i, max) {" +
                    "    return i + \"/\" + max + \": \"\" + row.Key + \"\" [\" + row.Value + \"]\";" +
                    "}";
            }
        }

        public static string CountEmailItemFormat
        {
            get
            {
                return "function(row, i, max) {" +
                    "    return i + \"/\" + max + \": \"\" + row.name + \"\" [\" + row.to + \"]\";" +
                    "}";
            }
        }

        #endregion

        #region MatchFormats

        public static string DefaultEmailMatchFormat
        {
            get
            {
                return "function(row, i, max) {" +
                    "    return row.name + \" \" + row.to;" +
                    "}";
            }
        }

        public static string DefaultKeyValueMatchFormat
        {
            get
            {
                return "function(row, i, max) {" +
                    "    return row.Key + \" \" + row.Value;" +
                    "}";
            }
        }

        #endregion

        #region ResultFormats

        public static string DefaultEmailResultFormat
        {
            get
            {
                return "function(row) {" +
                "    return row.to;" +
                "}";
            }
        }

        public static string DefaultKeyValueResultFormat
        {
            get
            {
                return "function(row) {" +
                "    return row.Value;" +
                "}";
            }
        }

        #endregion




        public static string ItemFormatFunction(string keyPropertyName, string valuePropertyName)
        {
            return "function(row, i, max) {" +
                    "    return \"\" + row." + keyPropertyName + " + \"\" + \"&lt;\" + row." + valuePropertyName + " + \"&gt;\";" +
                    "}";
        }

        internal static string ItemMatchFunction(string keyPropertyName, string valuePropertyName)
        {
            return "function(row, i, max) {" +
                "    return row." + keyPropertyName + " + \" \" + row." + valuePropertyName + ";" +
                "}";
        }

        internal static string ItemResultFunction(string keyPropertyName, string valuePropertyName)
        {
            return "function(row) {" +
                "    return row." + valuePropertyName + ";" +
                "}";
        }

        public static string ItemParseFunction(string keyPropertyName, string valuePropertyName)
        {
            return "function(data)  {  var rows = new Array();   for (var i = 0; i < data.length; i++) { rows[i] = { data: data[i], value: data[i]."+valuePropertyName+", result: data[i]."+valuePropertyName+" };   }  return rows; } ";
        }

        public static string TreeItemFormatFunction(string keyPropertyName, string valuePropertyName, string depthPropertyName)
        {
            return "function(row, i, max) {" +
                    "    var depth = row." + depthPropertyName +";"+
                    @"  
                        var space = '';
                    
                        for ( j=0; j < depth ; j++)
                        {
                            space += '&nbsp;' + '&nbsp;';}" +
                    
                    "   return \"\" + row." + keyPropertyName + " + \"\" + space + row." + valuePropertyName + ";" +
                    "}";
        }

        public static string TreeItemMatchFunction(string keyPropertyName, string valuePropertyName)
        {
            return "function(row, i, max) { " +
                "    return row." + keyPropertyName + " + \" \" + row." + valuePropertyName + ";" +
                "}";
        }

        public static string TreeItemResultFunction(string keyPropertyName, string valuePropertyName) //o que mostra após digitar o ente
        {
            return "function(row) { " +
                "    return row." + valuePropertyName + ";" +
                "}";
        }

        public static string TreeItemParseFunction(string keyPropertyName, string valuePropertyName)
        {
            return "function(data)  {  var rows = new Array();   for (var i = 0; i < data.length; i++) { rows[i] = { data: data[i], value: data[i]." + valuePropertyName + ", result: data[i]." + valuePropertyName + " };   }  return rows; } ";
        
        }
    }

    public class MosesGridModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Index { get; set; }
        public AlignOptions Align { get; set; }

        public enum AlignOptions
        {
            Left,
            Center,
            Right

        }

    }
}
