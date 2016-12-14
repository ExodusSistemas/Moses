using System;
using System.Web.Mvc;
using Trirand.Web.Mvc;

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

        public MvcHtmlString RenderGrid(JQGrid grid, string detailsPath = "Details", string detailsController = null)
        {
            //corrige a renderização padrão do JQGrid 
            var s = new TrirandNamespace().JQGrid(grid, grid.ID).ToHtmlString();

            var controllerExpression = "";
            if (detailsController != null)
                controllerExpression = "fn-controller=\"" + detailsController + "\" ";

            //hack para transformar uma execução inline em uma execução contextualizada
            s = s.Replace("\r", "").Replace("\n", "").Replace("<table ", "<table class=\"mosesGridInstance\"")
                .Replace("jQuery(document).ready(function() {jQuery('#" + grid.ID + "').jqGrid(", "$context.GridDefinitions['" + grid.ID + "'] = function($grid){ return ")
                .Replace("function jqGrid_aspnet_loadErrorHandler(xht, st, handler) {jQuery(document.body).css('font-size','100%'); jQuery(document.body).html(xht.responseText);}", "")
                .Replace(").bindKeys();", "")
                .Replace("});</script>", ";}</script>");


            return new MvcHtmlString(string.Concat(s, "<div id='" + grid.ID + "_grid-rowExpanded' class=\"grid-rowExpanded\" fn-type=\"ManualLoad\" fn-action=\"" + detailsPath + "\" " + controllerExpression + " ></div>"));
        }

        public MvcHtmlString RenderChart(JQChart chart, string detailsController = null)
        {
            //corrige a renderização padrão do JQGrid 
            var s = new TrirandNamespace().JQChart(chart, chart.ID).ToHtmlString();

            var controllerExpression = "";
            if (detailsController != null)
                controllerExpression = "fn-controller=\"" + detailsController + "\" ";

            //hack para transformar uma execução inline em uma execução contextualizada
            s = s.Replace("<div ", "<div class=\"mosesChartInstance\" ")
                .Replace("jQuery(document).ready(function() {var chart", "MosesChartModule.prototype.RenderChart = function($chart) { $chart.Current")
                .Replace("});</script>", "}</script>");

            return new MvcHtmlString(s);
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
