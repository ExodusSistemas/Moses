namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Runtime.CompilerServices;

    public class SortSettings
    {
        public SortSettings()
        {
            this.InitialSortColumn = "";
            this.InitialSortDirection = Moses.Web.Mvc.Controls.SortDirection.Asc;
            this.SortIconsPosition = Moses.Web.Mvc.Controls.SortIconsPosition.Vertical;
            this.SortAction = Moses.Web.Mvc.Controls.SortAction.ClickOnHeader;
        }

        public string InitialSortColumn { get; set; }

        public Moses.Web.Mvc.Controls.SortDirection InitialSortDirection { get; set; }

        public Moses.Web.Mvc.Controls.SortAction SortAction { get; set; }

        public Moses.Web.Mvc.Controls.SortIconsPosition SortIconsPosition { get; set; }
    }
}

