namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Runtime.CompilerServices;

    public class GroupField
    {
        public GroupField()
        {
            this.DataField = "";
            this.HeaderText = "<b>{0}</b>";
            this.ShowGroupColumn = true;
            this.GroupSortDirection = Moses.Web.Mvc.Controls.SortDirection.Asc;
            this.ShowGroupSummary = false;
        }

        public string DataField { get; set; }

        public Moses.Web.Mvc.Controls.SortDirection GroupSortDirection { get; set; }

        public string HeaderText { get; set; }

        public bool ShowGroupColumn { get; set; }

        public bool ShowGroupSummary { get; set; }
    }
}

