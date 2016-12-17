namespace Moses.Web.Mvc.Controls
{
    using System;

    internal static class JsonUtil
    {
        internal static string RenderClientSideEvent(string json, string jsName, string eventName)
        {
            string str = (json.Length > 2) ? "," : "";
            string str2 = "";
            if (!string.IsNullOrEmpty(eventName))
            {
                str2 = $"{str}{jsName}:{eventName}";
                return json.Insert(json.Length - 1, str2);
            }
            return json;
        }
    }
}

