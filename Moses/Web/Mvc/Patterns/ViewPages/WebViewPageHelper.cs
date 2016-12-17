using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;
using Moses.Web.Mvc.Controls;

namespace Moses.Web.Mvc.Patterns
{
    public static class HtmlHelpers
    {
        public static string ModuleName(this HtmlHelper htmlHelper)
        {
            string[] _moduleOptions = new[] { "Credenciados" };

            return GetController(htmlHelper)?.Session != null ? (GetController(htmlHelper)?.Session["Module"] != null ? GetController(htmlHelper)?.Session["Module"] as string : "Credenciados") : "Credenciados";
        }

        public static MembershipContext MembershipContext(this HtmlHelper htmlHelper)
        {
            return GetController(htmlHelper)?.MembershipContext;
        }

        private static MosesController GetController(HtmlHelper htmlHelper)
        {
            var controller = htmlHelper?.ViewContext?.Controller as MosesController;
            return controller;
        }

        public static string GetScriptControllerName(this WebViewPage webPage)
        {
            return System.IO.Path.GetFileNameWithoutExtension(webPage.Server.MapPath(((RazorView)webPage.ViewContext.View).ViewPath)) + "Controller";
        }
    }

    public static class WebViewPageHelper
    {

        public static string PublicRoute(this UrlHelper container, string url)
        {
            return container.Content("~/PublicRoute/App#!=" + url);
        }

        public static string Route(this UrlHelper container, string url)
        {
            return container.Content("~/Route/App#!=" + url);
        }

        /// <summary>
        /// Defines a type of action specific to work with module trespassing
        /// </summary>
        /// <param name="container"></param>
        /// <param name="ation"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static string Route(this UrlHelper container, string action, string controller, object routeValues = null)
        {
            var url = container.Action(action, controller, new { routeparams = routeValues });
            return container.Content("~/Route/App#!=" + url);
        }

        /// <summary>
        /// Executes a Fresh Route, reloading the AppContainer
        /// </summary>
        /// <param name="container"></param>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public static string FreshRoute(this UrlHelper container, string action, string controller, object routeValues = null)
        {
            var url = container.Action(action, controller, new { routeparams = routeValues });
            return container.Content("~/Route/Reload#!=" + url);
        }

        public static string PublicRoute(this UrlHelper container, string action, string controller, string routeValues = null)
        {
            var url = container.Action(action, controller, new { routeparams = routeValues });
            if (routeValues != null)
            {
                url = url.Insert(url.LastIndexOf("?routeparams") + 1, "&");
            }
            return container.Content("~/PublicRoute/App#!=" + url);
        }


        public static void OnLoadMasterPages(this Control masterPage)
        {

            //O usuário só pode ver um contrato
            //if (DdContratos.Items.Count == 1)
            //{
            //    BtEntrar_Command(null, null);
            //}

            masterPage.Page.Title = Moses.Web.Configuration.ApplicationConfiguration.ApplicationTitle;
        }
    }
}