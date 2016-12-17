namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using System.Web.UI.WebControls;

    public class TreeControl
    {
        public TreeControl()
        {
            this.ID = "";
            this.DataUrl = "";
            this.Width = Unit.Empty;
            this.Height = Unit.Empty;
        }

        public JsonResult DataBind(List<Moses.Web.Mvc.Controls.JQTreeNode> nodes)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = (0);
            result.Data = (Configuration.Json.Serialize(this.SerializeNodes(nodes)));
            return result;
        }

        private List<object> SerializeNodes(List<Moses.Web.Mvc.Controls.JQTreeNode> nodes)
        {
            List<object> list = new List<object>();
            foreach (Moses.Web.Mvc.Controls.JQTreeNode node in nodes)
            {
                List<object> item = new List<object> {
                    node.Text,
                    node.Value,
                    node.Url,
                    Convert.ToInt16(node.Expanded),
                    Convert.ToInt16(node.Enabled),
                    this.SerializeNodes(node.Nodes)
                };
                list.Add(item);
            }
            return list;
        }

        public string DataUrl { get; set; }

        public Unit Height { get; set; }

        public string ID { get; set; }

        public Unit Width { get; set; }
    }
}

