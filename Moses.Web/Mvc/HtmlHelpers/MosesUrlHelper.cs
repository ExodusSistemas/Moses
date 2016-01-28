using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc.Html;
using System.Collections.Specialized;
using System.Web;
using System.Globalization;

namespace Moses.Web.Mvc.Html
{
    public static class MosesUrlHelper
    {
        public static string GenerateUrl(string routeName, string actionName, string controllerName, RouteValueDictionary routeValues, RouteCollection routeCollection,HttpContextBase context,  bool includeImplicitMvcValues)
        {
            return GenerateUrl( routeName,  actionName,  controllerName,   routeValues,  routeCollection,  new RequestContext(context, routeCollection.GetRouteData(context)) ,  includeImplicitMvcValues);
        }


        public static string GenerateUrl(string routeName, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, RouteCollection routeCollection, RequestContext requestContext, bool includeImplicitMvcValues)
        {
            string url = GenerateUrl(routeName, actionName, controllerName, routeValues, routeCollection, requestContext, includeImplicitMvcValues);

            if (url != null)
            {
                if (!String.IsNullOrEmpty(fragment))
                {
                    url = url + "#" + fragment;
                }

                if (!String.IsNullOrEmpty(protocol) || !String.IsNullOrEmpty(hostName))
                {
                    Uri requestUrl = requestContext.HttpContext.Request.Url;
                    protocol = (!String.IsNullOrEmpty(protocol)) ? protocol : Uri.UriSchemeHttp;
                    hostName = (!String.IsNullOrEmpty(hostName)) ? hostName : requestUrl.Host;

                    string port = String.Empty;
                    string requestProtocol = requestUrl.Scheme;

                    if (String.Equals(protocol, requestProtocol, StringComparison.OrdinalIgnoreCase))
                    {
                        port = requestUrl.IsDefaultPort ? String.Empty : (":" + Convert.ToString(requestUrl.Port, CultureInfo.InvariantCulture));
                    }

                    url = protocol + Uri.SchemeDelimiter + hostName + port + url;
                }
            }

            return url;
        }

        public static string GenerateUrl(string routeName, string actionName, string controllerName, RouteValueDictionary routeValues, RouteCollection routeCollection, RequestContext requestContext, bool includeImplicitMvcValues)
        {
            RouteValueDictionary mergedRouteValues = MosesRouteValuesHelpers.MergeRouteValues(actionName, controllerName, requestContext.RouteData.Values, routeValues, includeImplicitMvcValues);

            VirtualPathData vpd = routeCollection.GetVirtualPath(requestContext, routeName, mergedRouteValues);
            if (vpd == null)
            {
                return null;
            }

            string modifiedUrl = MosesPathHelpers.GenerateClientUrl(requestContext.HttpContext, vpd.VirtualPath);
            return modifiedUrl;
        }
    }



    /// <summary>
    /// Classe RouteValuesHelpers
    /// </summary>
    /// <remarks>
    /// Enxerto de código estático vindo direto dos fontes do MVC. Necessário para o GenerateUrl
    /// </remarks>
    public static class MosesRouteValuesHelpers
    {
        public static RouteValueDictionary GetRouteValues(RouteValueDictionary routeValues)
        {
            return (routeValues != null) ? new RouteValueDictionary(routeValues) : new RouteValueDictionary();
        }

        public static RouteValueDictionary MergeRouteValues(string actionName, string controllerName, RouteValueDictionary implicitRouteValues, RouteValueDictionary routeValues, bool includeImplicitMvcValues)
        {
            // Create a new dictionary containing implicit and auto-generated values
            RouteValueDictionary mergedRouteValues = new RouteValueDictionary();

            if (includeImplicitMvcValues)
            {
                // We only include MVC-specific values like 'controller' and 'action' if we are generating an action link.
                // If we are generating a route link [as to MapRoute("Foo", "any/url", new { controller = ... })], including
                // the current controller name will cause the route match to fail if the current controller is not the same
                // as the destination controller.

                object implicitValue;
                if (implicitRouteValues != null && implicitRouteValues.TryGetValue("action", out implicitValue))
                {
                    mergedRouteValues["action"] = implicitValue;
                }

                if (implicitRouteValues != null && implicitRouteValues.TryGetValue("controller", out implicitValue))
                {
                    mergedRouteValues["controller"] = implicitValue;
                }
            }

            // Merge values from the user's dictionary/object
            if (routeValues != null)
            {
                foreach (KeyValuePair<string, object> routeElement in GetRouteValues(routeValues))
                {
                    mergedRouteValues[routeElement.Key] = routeElement.Value;
                }
            }

            // Merge explicit parameters when not null
            if (actionName != null)
            {
                mergedRouteValues["action"] = actionName;
            }

            if (controllerName != null)
            {
                mergedRouteValues["controller"] = controllerName;
            }

            return mergedRouteValues;
        }
    }

    internal static class MosesPathHelpers
    {

        private const string _urlRewriterServerVar = "HTTP_X_ORIGINAL_URL";

        // this method can accept an app-relative path or an absolute path for contentPath
        public static string GenerateClientUrl(HttpContextBase httpContext, string contentPath)
        {
            if (String.IsNullOrEmpty(contentPath))
            {
                return contentPath;
            }

            // many of the methods we call internally can't handle query strings properly, so just strip it out for
            // the time being
            string query;
            contentPath = StripQuery(contentPath, out query);

            return GenerateClientUrlInternal(httpContext, contentPath) + query;
        }

        private static string GenerateClientUrlInternal(HttpContextBase httpContext, string contentPath)
        {
            if (String.IsNullOrEmpty(contentPath))
            {
                return contentPath;
            }

            // can't call VirtualPathUtility.IsAppRelative since it throws on some inputs
            bool isAppRelative = contentPath[0] == '~';
            if (isAppRelative)
            {
                string absoluteContentPath = VirtualPathUtility.ToAbsolute(contentPath, httpContext.Request.ApplicationPath);
                string modifiedAbsoluteContentPath = httpContext.Response.ApplyAppPathModifier(absoluteContentPath);
                return GenerateClientUrlInternal(httpContext, modifiedAbsoluteContentPath);
            }

            // we only want to manipulate the path if URL rewriting is active, else we risk breaking the generated URL
            NameValueCollection serverVars = httpContext.Request.ServerVariables;
            bool urlRewriterIsEnabled = (serverVars != null && serverVars[_urlRewriterServerVar] != null);
            if (!urlRewriterIsEnabled)
            {
                return contentPath;
            }

            // Since the rawUrl represents what the user sees in his browser, it is what we want to use as the base
            // of our absolute paths. For example, consider mysite.example.com/foo, which is internally
            // rewritten to content.example.com/mysite/foo. When we want to generate a link to ~/bar, we want to
            // base it from / instead of /foo, otherwise the user ends up seeing mysite.example.com/foo/bar,
            // which is incorrect.
            string relativeUrlToDestination = MakeRelative(httpContext.Request.Path, contentPath);
            string absoluteUrlToDestination = MakeAbsolute(httpContext.Request.RawUrl, relativeUrlToDestination);
            return absoluteUrlToDestination;
        }

        public static string MakeAbsolute(string basePath, string relativePath)
        {
            // The Combine() method can't handle query strings on the base path, so we trim it off.
            string query;
            basePath = StripQuery(basePath, out query);
            return VirtualPathUtility.Combine(basePath, relativePath);
        }

        public static string MakeRelative(string fromPath, string toPath)
        {
            string relativeUrl = VirtualPathUtility.MakeRelative(fromPath, toPath);
            if (String.IsNullOrEmpty(relativeUrl) || relativeUrl[0] == '?')
            {
                // Sometimes VirtualPathUtility.MakeRelative() will return an empty string when it meant to return '.',
                // but links to {empty string} are browser dependent. We replace it with an explicit path to force
                // consistency across browsers.
                relativeUrl = "./" + relativeUrl;
            }
            return relativeUrl;
        }

        private static string StripQuery(string path, out string query)
        {
            int queryIndex = path.IndexOf('?');
            if (queryIndex >= 0)
            {
                query = path.Substring(queryIndex);
                return path.Substring(0, queryIndex);
            }
            else
            {
                query = null;
                return path;
            }
        }

    }


}
