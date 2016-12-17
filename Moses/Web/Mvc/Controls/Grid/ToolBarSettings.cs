namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ToolBarSettings
    {
        public ToolBarSettings()
        {
            this.ShowEditButton = false;
            this.ShowAddButton = false;
            this.ShowDeleteButton = false;
            this.ShowSearchButton = false;
            this.ShowRefreshButton = false;
            this.ShowViewRowDetailsButton = false;
            this.ShowSearchToolBar = false;
            this.ToolBarAlign = Moses.Web.Mvc.Controls.ToolBarAlign.Left;
            this.ToolBarPosition = Moses.Web.Mvc.Controls.ToolBarPosition.Bottom;
            this.CustomButtons = new List<Moses.Web.Mvc.Controls.JQGridToolBarButton>();
        }

        public List<Moses.Web.Mvc.Controls.JQGridToolBarButton> CustomButtons { get; set; }

        public bool ShowAddButton { get; set; }

        public bool ShowDeleteButton { get; set; }

        public bool ShowEditButton { get; set; }

        public bool ShowRefreshButton { get; set; }

        public bool ShowSearchButton { get; set; }

        public bool ShowSearchToolBar { get; set; }

        public bool ShowViewRowDetailsButton { get; set; }

        public Moses.Web.Mvc.Controls.ToolBarAlign ToolBarAlign { get; set; }

        public Moses.Web.Mvc.Controls.ToolBarPosition ToolBarPosition { get; set; }
    }
}

