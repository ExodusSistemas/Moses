namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Runtime.CompilerServices;

    public class SearchToolBarSettings
    {
        public SearchToolBarSettings()
        {
            this.SearchToolBarAction = Moses.Web.Mvc.Controls.SearchToolBarAction.SearchOnEnter;
        }

        public Moses.Web.Mvc.Controls.SearchToolBarAction SearchToolBarAction { get; set; }
    }
}

