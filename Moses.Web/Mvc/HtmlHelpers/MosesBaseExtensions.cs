using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moses.Web.Mvc.Html
{
    public static class MosesBaseExtensions
    {
        private static readonly string keyValueHtmlAttFormat = " {0}=\"{1}\"";

        public static void AddAttribute(this StringBuilder builder, string key, string value)
        {
            builder.AppendFormat(keyValueHtmlAttFormat, key, value);
        }

        public static void AppendScript(this StringBuilder builder, string script, params object[] args)
        {
            string formatedString = string.Format( script,args);
            builder.AppendFormat("<script type=\"text/javascript\" >{0}</script>", formatedString);
        }

        public static void AppendScript(this StringBuilder builder, string script)
        {
            builder.AppendFormat("<script type=\"text/javascript\" >{0}</script>",script);
        }


    }
}
