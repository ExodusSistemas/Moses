using System.Web;
using System;
using Moses;
using System.Web.UI;
using System.Text;
using System.Collections;

namespace Moses.Web
{
    public static class HttpHelpers
    {
        public static bool IsNullOrEmptyKey(this HttpRequest request, string key) 
        {
            return string.IsNullOrEmpty(request[key]);
        }

        public static bool IsNullOrEmptyKey(this StateBag viewstate, string key)
        {
            if (viewstate[key] != null) return true;
            return string.IsNullOrEmpty(viewstate[key].ToString());
        }

        public static int? GetIntKey(this HttpRequest request, string key) 
        {
            
            if (!request.IsNullOrEmptyKey(key))
            {
                try
                {
                    if ( request[key].IsNumber() )
                        return int.Parse(request[key]);
                    return null;
                }
                catch
                {
                    return null;
                }
            }
            else{
                return null;
            }
        
            //if (typeof(T) == typeof(decimal) || typeof(T) == typeof(Nullable<decimal>))
            //{
            //    try
            //    {
            //        return Convert.ChangeType(decimal.Parse(request[key], typeof(T) );
            //    }
            //    catch
            //    {
            //        return default(T);
            //    }
            //}
            //if (typeof(T) == typeof(DateTime) || typeof(T) == typeof(Nullable<DateTime>))
            //{
            //    try
            //    {
            //        return DateTime.Parse(request[key]);
            //    }
            //    catch
            //    {
            //        return default(T);
            //    }
            //}
            //if (typeof(T) == typeof(long) || typeof(T) == typeof(Nullable<long>))
            //{
            //    try
            //    {
            //        return (T) long.Parse(request[key]);
            //    }
            //    catch
            //    {
            //        return default(T);
            //    }
            //}
            //else
            //{
            //    throw;
            //}
        }

        /// <summary>
        /// Returns a site relative HTTP path from a partial path starting out with a ~.
        /// Same syntax that ASP.Net internally supports but this method can be used
        /// outside of the Page framework.
        /// 
        /// Works like Control.ResolveUrl including support for ~ syntax
        /// but returns an absolute URL.
        /// </summary>
        /// <param name="originalUrl">Any Url including those starting with ~</param>
        /// <returns>relative url</returns>
        public static string ResolveUrl(string originalUrl)
        {
            if (originalUrl == null)
                return null;
         
            // *** Absolute path - just return
            if (originalUrl.IndexOf("://") != -1)
                return originalUrl;
         
            // *** Fix up image path for ~ root app dir directory
            if (originalUrl.StartsWith("~"))
            {
                string newUrl = "";
                if (HttpContext.Current != null)
                    newUrl = HttpContext.Current.Request.ApplicationPath +
                          originalUrl.Substring(1).Replace("//", "/");
                else
                    // *** Not context: assume current directory is the base directory
                    throw new ArgumentException("Invalid URL: Relative URL not allowed.");                                       
         
                // *** Just to be sure fix up any double slashes
                return newUrl;
            }
         
            return originalUrl;
        }
         
        /// <summary>
        /// This method returns a fully qualified absolute server Url which includes
        /// the protocol, server, port in addition to the server relative Url.
        /// 
        /// Works like Control.ResolveUrl including support for ~ syntax
        /// but returns an absolute URL.
        /// </summary>
        /// <param name="ServerUrl">Any Url, either App relative or fully qualified</param>
        /// <param name="forceHttps">if true forces the url to use https</param>
        /// <returns></returns>
        public static string ResolveServerUrl(string serverUrl, bool forceHttps)
        {
            // *** Is it already an absolute Url?
            if (serverUrl.IndexOf("://") > -1)
                return serverUrl;
         
            // *** Start by fixing up the Url an Application relative Url
            string newUrl = ResolveUrl(serverUrl);
         
            Uri originalUri = HttpContext.Current.Request.Url;
            newUrl = (forceHttps ? "https" : originalUri.Scheme) + 
                     "://" + originalUri.Authority + newUrl;
         
            return newUrl;
        } 
         
        /// <summary>
        /// This method returns a fully qualified absolute server Url which includes
        /// the protocol, server, port in addition to the server relative Url.
        /// 
        /// It work like Page.ResolveUrl, but adds these to the beginning.
        /// This method is useful for generating Urls for AJAX methods
        /// </summary>
        /// <param name="ServerUrl">Any Url, either App relative or fully qualified</param>
        /// <returns></returns>
        public static string ResolveServerUrl(string serverUrl)
        {
            return ResolveServerUrl(serverUrl, false);
        }



        public static Control FindControlRecursive(this Control parent, string toFind)
        {
            Control found = parent.FindControl(toFind);
            if (found == null)
            {
                foreach (Control c in parent.Controls)
                {
                    Control ctl = FindControlRecursive(c, toFind);
                    if (ctl != null)
                    {
                        found = ctl;
                        break;
                    }
                }
            }
            return found;
        }

        //private static T NewMethod<T, K>(HttpRequest request, string key) where T : IConvertible 
        //    where K : IParseable
        //{
        //    try
        //    {
        //        return (T)Convert.ChangeType(int.Parse(request[key]), typeof(T));
        //    }
        //    catch
        //    {
        //        return default(T);
        //    }
        //}
    }



}