namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Collections;
    using System.Web.Script.Serialization;

    internal class JsonSearchToolBar
    {
        private Moses.Web.Mvc.Controls.GridControl _grid;
        private Hashtable _jsonValues = new Hashtable();

        public JsonSearchToolBar(Moses.Web.Mvc.Controls.GridControl grid)
        {
            this._grid = grid;
        }

        public string Process()
        {
            if (this._grid.SearchToolBarSettings.SearchToolBarAction == Moses.Web.Mvc.Controls.SearchToolBarAction.SearchOnKeyPress)
            {
                this._jsonValues["searchOnEnter"] = false;
            }
            return Configuration.Json.Serialize(this._jsonValues);
        }
    }
}

