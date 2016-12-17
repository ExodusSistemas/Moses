namespace Moses.Extensions
{
    using System;
    using System.ComponentModel;
    using System.Web.Mvc;

    public class MosesNamespace
    {
        public MvcHtmlString AutoCompleteControl(Moses.Web.Mvc.Controls.AutoCompleteControl autoComplete, string id)
        {
            Moses.Web.Mvc.Controls.AutoCompleteRenderer renderer = new Moses.Web.Mvc.Controls.AutoCompleteRenderer(autoComplete);
            autoComplete.ID = id;
            return MvcHtmlString.Create(renderer.RenderHtml());
        }

        public MvcHtmlString DatePickerControl(Moses.Web.Mvc.Controls.DatePickerControl datePicker, string id)
        {
            Moses.Web.Mvc.Controls.DatePickerRenderer renderer = new Moses.Web.Mvc.Controls.DatePickerRenderer(datePicker);
            datePicker.ID = id;
            return MvcHtmlString.Create(renderer.RenderHtml());
        }

        public MvcHtmlString GridControl(Moses.Web.Mvc.Controls.GridControl grid, string id, string detailsPath = "Details", string detailsController = null)
        {
            Moses.Web.Mvc.Controls.GridRenderer renderer = new Moses.Web.Mvc.Controls.GridRenderer();
            grid.ID = id;
            return MvcHtmlString.Create(renderer.RenderHtml(grid, detailsPath, detailsController));
        }

        public MvcHtmlString TreeControl(Moses.Web.Mvc.Controls.TreeControl tree, string id)
        {
            Moses.Web.Mvc.Controls.TreeRenderer renderer = new Moses.Web.Mvc.Controls.TreeRenderer(tree);
            tree.ID = id;
            return MvcHtmlString.Create(renderer.RenderHtml());
        }

    }
}

