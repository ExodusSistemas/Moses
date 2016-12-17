namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.Script.Serialization;

    internal static class StringExtensions
    {
        internal static string RemoveQuotes(this string buffer, string expression)
        {
            new JavaScriptSerializer();
            return buffer.Replace("\\\"" + expression + "\\\"", expression);
        }
    }
}

