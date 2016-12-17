namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Runtime.CompilerServices;

    public class HierarchySettings
    {
        public HierarchySettings()
        {
            this.HierarchyMode = Moses.Web.Mvc.Controls.HierarchyMode.None;
        }

        public Moses.Web.Mvc.Controls.HierarchyMode HierarchyMode { get; set; }
    }
}

