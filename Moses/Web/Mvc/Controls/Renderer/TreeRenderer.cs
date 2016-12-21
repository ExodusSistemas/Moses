namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Text;
    using System.Web.Script.Serialization;

    internal class TreeRenderer
    {
        private Moses.Web.Mvc.Controls.TreeControl _model;

        public TreeRenderer(Moses.Web.Mvc.Controls.TreeControl model)
        {
            this._model = model;
        }

        private string GetStandaloneJavascript()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("<ul id='{0}'></ul>", this._model.ID);
            builder.Append("<script type='text/javascript'>\n");
            builder.Append("jQuery(document).ready(function() {");
            builder.AppendFormat("jQuery('#{0}').jqTree({{", this._model.ID);
            builder.Append(this.GetStartupOptions());
            builder.Append("});");
            builder.Append("});");
            builder.Append("</script>");
            return builder.ToString();
        }

        private string GetStartupOptions()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("id: '{0}'", this._model.ID);
            builder.AppendFormat("url: '{0}'", this._model.DataUrl);
            return builder.ToString();
        }

        public string RenderHtml()
        {
            Moses.Web.Mvc.Controls.Guard.IsNotNullOrEmpty(this._model.ID, "ID", "You need to set ID for this JQTree instance.");
            Moses.Web.Mvc.Controls.Guard.IsNotNullOrEmpty(this._model.DataUrl, "DataUrl", "You need to set DataUrl to the Action of the tree returning nodes.");
            return this.GetStandaloneJavascript();
        }
    }
}

