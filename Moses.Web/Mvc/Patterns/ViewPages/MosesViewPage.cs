using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Mvc;
using System.Text;
using System.IO;
using Trirand.Web.Mvc;

namespace Moses.Web.Mvc.Patterns
{
    public enum MosesWebViewType
    {
        List,
        Form,
        Report,
        Embeded,
        None,
        
    }

    public enum SearchTheme
    {
        Standard,
        Lite,
        Invisible
    }

    public class MosesWebViewPage<dynamic> : WebViewPage<dynamic> , IMosesPermissionContainer
    {
        public MembershipContext MembershipContext{ get;set;}
        public MosesWebViewType ViewType { get; set; }

        public string FnControllerName { get; set; }

        public bool DefaultFullView { get; set; }


        public MosesWebViewPage()
        {
            MembershipContext = MembershipContextFactory.Initialize(new HttpContextWrapper(HttpContext.Current));
            DefaultFullView = true;
        }

        public override void InitHelpers()
        {
            base.InitHelpers();

            //Define o nome padrão do controller principal
            this.ViewBag.FnControllerName = Path.GetFileNameWithoutExtension(Server.MapPath(VirtualPath)) + "Controller";
            
        }

        public override void Execute()
        {
            
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


            return new MvcHtmlString(string.Concat(s, "<div class=\"grid-rowExpanded\" fn-action=\"" + detailsPath + "\" " + controllerExpression + " ></div>"));
        }

        public MvcHtmlString RenderChart(JQChart chart, string detailsController= null)
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

        public void DefineLayout(MosesWebViewType type, bool fullView = true, string nonAjaxLayout = "~/Views/Shared/_Layout.cshtml")
        {
            ViewBag.ViewType = type.ToString();
            if (type == MosesWebViewType.List)
            {
                this.DefineSearch(true, false, SearchTheme.Standard);
                if (!this.IsAjax)
                {
                    this.Layout = nonAjaxLayout;
                }
                else
                {
                    this.Layout = "";
                }
            }
            else if (type == MosesWebViewType.Form)
            {
                if (!this.IsAjax)
                {
                    this.Layout = nonAjaxLayout;
                }
                else
                {
                    ViewBag.FnView = "Form";
                    this.Layout = "~/Views/Shared/_AjaxFormLayout.cshtml";
                    
                }
            }
            else if (type == MosesWebViewType.Report)
            {
                this.DefineSearch(true, true, SearchTheme.Invisible);
                if (!this.IsAjax)
                {
                    
                    this.Layout = nonAjaxLayout;
                }
                else
                {
                    this.Layout = "";
                }
            }
            else if (type == MosesWebViewType.None)
            {
                if (!this.IsAjax)
                {
                    this.Layout = nonAjaxLayout;
                }
                else
                {
                    this.Layout = "";
                }
            };
            ViewBag.DefaultFullView = fullView;
        }

        //propriedades de visualização do formulário de busca
        public void DefineSearch(bool detached, bool extended, SearchTheme theme = SearchTheme.Standard)
        {
            ViewBag.ShowHeader = theme != SearchTheme.Invisible;
            ViewBag.Theme = theme;
            ViewBag.SearchThemeClass = theme == SearchTheme.Standard ? "blue" : "";
            ViewBag.SearchFormDetached = detached;
            ViewBag.SearchFormExtended = extended;
        }

       
        #region IMosesPermissionContainer Members

        MosesPermissionContainer _permissionContainer;

        public IMosesPermission Permission
        {
            get
            {

                if (_permissionContainer == null)
                {
                    _permissionContainer = new MosesPermissionContainer(MembershipContext);
                }
                return _permissionContainer.GetPermissionSet();
            }
        }

        public void Update()
        {
            if (_permissionContainer != null)
            {
                _permissionContainer._updatePermissionsExecuted = true;
            }
        }

        #endregion
    }

    
}
