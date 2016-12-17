using System;
using System.Web.Mvc;
using Moses.Web.Mvc.Controls;

namespace Moses.Web.Mvc.Patterns
{
    public enum WebViewType
    {
        List,
        Form,
        Report,
        Embeded,
        None,
        App,
        ReportParam,
        Empty,
        PublicApp
    }

    public enum MosesWebPageSearchTheme
    {
        Standard,
        Lite,
        Invisible
    }

    public class MosesWebViewPage<dynamic> : WebViewPage<dynamic>
    {
        public class MosesDefaultViews
        {
            public static readonly string EmptyLayout = "_AjaxEmptyLayout";
            public static readonly string ListLayout = "_AjaxListLayout";
            public static readonly string FormLayout = "_AjaxFormLayout";
            public static readonly string ReportLayout = "_AjaxReportLayout";
            public static readonly string PublicLayout = "_Public";
            public static readonly string DefaultLayout = "_Layout";
        }

        MembershipContext _membershipContext = null;
        public MembershipContext MembershipContext
        {
            get
            {
                if (_membershipContext == null)
                {
                    _membershipContext = new MembershipContext(new System.Web.HttpContextWrapper(System.Web.HttpContext.Current));
                }
                return _membershipContext;
            }
        }

        public WebViewType ViewType { get; set; }

        public string FnControllerName { get; set; }

        public bool DefaultFullView { get; set; }

        public MosesWebViewPage()
        {
            DefaultFullView = true;
        }

        public override void Execute()
        {

        }

        private string GetView(string viewName)
        {
            return string.Format("~/Views/Shared/{0}.cshtml", viewName);
        }

        

        protected virtual void OnDefineBackground(WebViewType type)
        {
        }

        public void DefineLayout(WebViewType type, bool fullView = true)
        {
            ViewBag.ViewType = type.ToString();
            string nonAjaxLayout = GetDefaultLayout(WebViewType.PublicApp == type);

            //Background Padrão
            this.OnDefineBackground(type);

            if (type == WebViewType.App || type == WebViewType.PublicApp)
            {
                this.DefineSearch(true, false, MosesWebPageSearchTheme.Standard);
                this.Layout = nonAjaxLayout;
            }
            else if (type == WebViewType.Empty)
            {
                this.Layout = GetView(MosesDefaultViews.EmptyLayout);
            }
            else if (type == WebViewType.List)
            {
                this.DefineSearch(true, false, MosesWebPageSearchTheme.Standard);
                if (!this.IsAjax)
                {
                    this.Layout = nonAjaxLayout;
                }
                else
                {
                    this.Layout = GetView(MosesDefaultViews.ListLayout);
                }
            }
            else if (type == WebViewType.Form)
            {
                if (!this.IsAjax)
                {
                    this.Layout = nonAjaxLayout;
                }
                else
                {
                    ViewBag.FnView = "Form";
                    this.Layout = GetView(MosesDefaultViews.FormLayout);
                }
            }
            else if (type == WebViewType.Report)
            {
                this.DefineSearch(true, true, MosesWebPageSearchTheme.Invisible);
                if (!this.IsAjax)
                {

                    this.Layout = nonAjaxLayout;
                }
                else
                {
                    ViewBag.FnView = "Report";
                    this.Layout = GetView(MosesDefaultViews.ReportLayout);
                }
            }
            else if (type == WebViewType.ReportParam)
            {
                if (!this.IsAjax)
                {
                    this.Layout = nonAjaxLayout;
                }
                else
                {
                    this.Layout = GetView(MosesDefaultViews.FormLayout);
                }
            }
            else if (type == WebViewType.None)
            {
                this.Layout = null;
            };
            ViewBag.DefaultFullView = fullView;
        }

        private string GetDefaultLayout(bool isPublic)
        {
            //var moduleName = Session["Module"] ?? "Credenciados";

            if (isPublic)
            {
                return GetView("_Public"); ;
                //return "~/Views/Shared/" + moduleName + "/_Public.cshtml";
            }
            else
            {
                return GetView("_Layout");
                //return "~/Views/Shared/" + moduleName + "/_Public.cshtml";
            }
        }

        //propriedades de visualização do formulário de busca
        public void DefineSearch(bool detached, bool extended, MosesWebPageSearchTheme theme = MosesWebPageSearchTheme.Standard)
        {
            ViewBag.ShowHeader = theme != MosesWebPageSearchTheme.Invisible;
            ViewBag.Theme = theme;
            ViewBag.SearchThemeClass = theme == MosesWebPageSearchTheme.Standard ? "green" : "";
            ViewBag.SearchFormDetached = detached;
            ViewBag.SearchFormExtended = extended;
        }



    }


}
