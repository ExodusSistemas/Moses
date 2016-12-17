namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Text;
    using System.Web.Script.Serialization;

    internal class AutoCompleteRenderer
    {
        private Moses.Web.Mvc.Controls.AutoCompleteControl _model;

        public AutoCompleteRenderer(Moses.Web.Mvc.Controls.AutoCompleteControl autoComplete)
        {
            this._model = autoComplete;
        }

        private string GetControlEditorJavascript() => 
            $"<script type='text/javascript'>var {this._model.ID}_acid = {{ {this.GetStartupOptions()} }};</script>";

        private string GetStandaloneJavascript()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("<input type='text' id='{0}' name='{0}' />", this._model.ID);
            builder.Append("<script type='text/javascript'>\n");
            builder.Append("jQuery(document).ready(function() {");
            builder.AppendFormat("jQuery('#{0}').autocomplete({{", this._model.ID);
            builder.Append(this.GetStartupOptions());
            builder.Append("});");
            builder.Append("});");
            builder.Append("</script>");
            return builder.ToString();
        }

        private string GetStartupOptions()
        {
            new JavaScriptSerializer();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("id: '{0}'", this._model.ID);
            sb.AppendFormat(",source: '{0}'", this._model.DataUrl);
            sb.AppendFormatIfTrue(this._model.Delay != 300, ",delay: {0}", new object[] { this._model.Delay });
            sb.AppendIfFalse(this._model.Enabled, ",disabled: true");
            sb.AppendFormatIfTrue(this._model.MinLength != 1, ",minLength: {0}", new object[] { this._model.MinLength });
            return sb.ToString();
        }

        public string RenderHtml()
        {
            Moses.Web.Mvc.Controls.Guard.IsNotNullOrEmpty(this._model.ID, "ID", "You need to set ID for this JQAutoComplete instance.");
            if (this._model.DisplayMode == Moses.Web.Mvc.Controls.AutoCompleteDisplayMode.Standalone)
            {
                return this.GetStandaloneJavascript();
            }
            return this.GetControlEditorJavascript();
        }
    }
}

