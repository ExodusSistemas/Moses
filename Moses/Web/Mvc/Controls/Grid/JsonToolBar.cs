namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Runtime.CompilerServices;

    internal class JsonToolBar
    {
        public JsonToolBar(Moses.Web.Mvc.Controls.ToolBarSettings settings)
        {
            this.edit = settings.ShowEditButton;
            this.add = settings.ShowAddButton;
            this.del = settings.ShowDeleteButton;
            this.search = settings.ShowSearchButton;
            this.refresh = settings.ShowRefreshButton;
            this.view = settings.ShowViewRowDetailsButton;
            this.position = settings.ToolBarAlign.ToString().ToLower();
            this.cloneToTop = true;
        }

        public bool add { get; set; }

        public bool cloneToTop { get; set; }

        public bool del { get; set; }

        public bool edit { get; set; }

        public string position { get; set; }

        public bool refresh { get; set; }

        public bool search { get; set; }

        public bool view { get; set; }
    }
}

