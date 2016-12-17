namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class JQTreeNode
    {
        public JQTreeNode()
        {
            this.Text = "";
            this.Value = "";
            this.Nodes = new List<Moses.Web.Mvc.Controls.JQTreeNode>();
            this.Expanded = false;
            this.Enabled = true;
            this.Url = "";
        }

        public bool Enabled { get; set; }

        public bool Expanded { get; set; }

        public List<Moses.Web.Mvc.Controls.JQTreeNode> Nodes { get; set; }

        public string Text { get; set; }

        public string Url { get; set; }

        public string Value { get; set; }
    }
}

