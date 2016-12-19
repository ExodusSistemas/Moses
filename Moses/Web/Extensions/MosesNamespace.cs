namespace Moses.Extensions
{
    using System;
    using System.ComponentModel;
    using System.Web.Mvc;

    public static class MosesNamespace
    {
        public static MvcHtmlString AutoCompleteControl(this HtmlHelper helper, Moses.Web.Mvc.Controls.AutoCompleteControl autoComplete, string id)
        {
            Moses.Web.Mvc.Controls.AutoCompleteRenderer renderer = new Moses.Web.Mvc.Controls.AutoCompleteRenderer(autoComplete);
            autoComplete.ID = id;
            return MvcHtmlString.Create(renderer.RenderHtml());
        }

        public static MvcHtmlString DatePickerControl(this HtmlHelper helper, Moses.Web.Mvc.Controls.DatePickerControl datePicker, string id)
        {
            Moses.Web.Mvc.Controls.DatePickerRenderer renderer = new Moses.Web.Mvc.Controls.DatePickerRenderer(datePicker);
            datePicker.ID = id;
            return MvcHtmlString.Create(renderer.RenderHtml());
        }

        public static MvcHtmlString GridControl(this HtmlHelper helper, Moses.Web.Mvc.Controls.GridControl grid, string id = null, string detailsPath = "Details", string detailsController = null)
        {
            Moses.Web.Mvc.Controls.GridRenderer renderer = new Moses.Web.Mvc.Controls.GridRenderer();
            grid.ID = id ?? grid.ID;

            if (grid.ID == null)
                throw new InvalidOperationException("The Grid Id must be not null");

            return MvcHtmlString.Create(renderer.RenderHtml(grid, detailsPath, detailsController));
        }

        public static MvcHtmlString TreeControl(this HtmlHelper helper, Moses.Web.Mvc.Controls.TreeControl tree, string id)
        {
            Moses.Web.Mvc.Controls.TreeRenderer renderer = new Moses.Web.Mvc.Controls.TreeRenderer(tree);
            tree.ID = id;
            return MvcHtmlString.Create(renderer.RenderHtml());
        }

    }
}

