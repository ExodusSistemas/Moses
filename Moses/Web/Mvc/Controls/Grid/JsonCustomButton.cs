namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Collections;
    using System.Text;
    using System.Web.Script.Serialization;

    internal class JsonCustomButton
    {
        private Moses.Web.Mvc.Controls.JQGridToolBarButton _button;
        private JavaScriptSerializer _jsonSerializer = new JavaScriptSerializer();
        private Hashtable _jsonValues = new Hashtable();

        public JsonCustomButton(Moses.Web.Mvc.Controls.JQGridToolBarButton button)
        {
            this._button = button;
        }

        public string Process()
        {
            string str = string.IsNullOrEmpty(this._button.Text) ? " " : this._button.Text;
            if (!string.IsNullOrEmpty(this._button.Text))
            {
                this._jsonValues["caption"] = str;
            }
            if (!string.IsNullOrEmpty(this._button.ButtonIcon))
            {
                this._jsonValues["buttonicon"] = this._button.ButtonIcon;
            }
            this._jsonValues["position"] = this._button.Position.ToString().ToLower();
            if (!string.IsNullOrEmpty(this._button.ToolTip))
            {
                this._jsonValues["title"] = this._button.ToolTip;
            }
            string json = this._jsonSerializer.Serialize(this._jsonValues);
            StringBuilder sb = new StringBuilder();
            this.RenderClientSideEvent(json, sb, "onClickButton", this._button.OnClick);
            return json.Insert(json.Length - 1, sb.ToString());
        }

        private void RenderClientSideEvent(string json, StringBuilder sb, string jsName, string eventName)
        {
            if (!string.IsNullOrEmpty(eventName))
            {
                sb.AppendFormat(",{0}:function() {{ {1}(); }}", jsName, eventName);
            }
        }
    }
}

