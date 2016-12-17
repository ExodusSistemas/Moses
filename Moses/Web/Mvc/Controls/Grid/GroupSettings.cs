namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public sealed class GroupSettings
    {
        public GroupSettings()
        {
            this.CollapseGroups = false;
            this.GroupFields = new List<Moses.Web.Mvc.Controls.GroupField>();
        }

        public bool CollapseGroups { get; set; }

        public List<Moses.Web.Mvc.Controls.GroupField> GroupFields { get; set; }
    }
}

