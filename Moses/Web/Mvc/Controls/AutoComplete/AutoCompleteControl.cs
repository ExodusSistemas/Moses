namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Runtime.CompilerServices;
    using System.Web;
    using System.Web.Mvc;

    public class AutoCompleteControl
    {
        public AutoCompleteControl()
        {
            this.AutoCompleteMode = Moses.Web.Mvc.Controls.AutoCompleteMode.BeginsWith;
            this.DataField = "";
            this.DataSource = null;
            this.DataUrl = "";
            this.Delay = 300;
            this.DisplayMode = Moses.Web.Mvc.Controls.AutoCompleteDisplayMode.Standalone;
            this.Enabled = true;
            this.ID = "";
            this.MinLength = 1;
        }

        public JsonResult DataBind() => 
            this.GetJsonResponse();

        public JsonResult DataBind(object dataSource)
        {
            this.DataSource = dataSource;
            return this.DataBind();
        }

        private JsonResult GetJsonResponse()
        {
            Moses.Web.Mvc.Controls.Guard.IsNotNull(this.DataSource, "DataSource");
            IQueryable dataSource = this.DataSource as IQueryable;
            Moses.Web.Mvc.Controls.Guard.IsNotNull(dataSource, "DataSource", "should implement the IQueryable interface.");
            Moses.Web.Mvc.Controls.Guard.IsNotNullOrEmpty(this.DataField, "DataField", "should be set to the datafield (column) of the datasource to search in.");
            Moses.Web.Mvc.Controls.SearchOperation isEqualTo = Moses.Web.Mvc.Controls.SearchOperation.IsEqualTo;
            if (this.AutoCompleteMode == Moses.Web.Mvc.Controls.AutoCompleteMode.BeginsWith)
            {
                isEqualTo = Moses.Web.Mvc.Controls.SearchOperation.BeginsWith;
            }
            else
            {
                isEqualTo = Moses.Web.Mvc.Controls.SearchOperation.Contains;
            }
            string str = HttpContext.Current.Request.QueryString["term"];
            if (!string.IsNullOrEmpty(str))
            {
                Moses.Web.Mvc.Controls.Util.SearchArguments args = new Moses.Web.Mvc.Controls.Util.SearchArguments {
                    SearchColumn = this.DataField,
                    SearchOperation = isEqualTo,
                    SearchString = str
                };
                dataSource = dataSource.Where(Moses.Web.Mvc.Controls.Util.ConstructLinqFilterExpression(this, args), new object[0]);
            }
            JsonResult result2 = new JsonResult();
            result2.JsonRequestBehavior = 0;
            result2.Data = dataSource.ToListOfString(this);
            return result2;
        }

        public Moses.Web.Mvc.Controls.AutoCompleteMode AutoCompleteMode { get; set; }

        public string DataField { get; set; }

        public object DataSource { get; set; }

        public string DataUrl { get; set; }

        public int Delay { get; set; }

        public Moses.Web.Mvc.Controls.AutoCompleteDisplayMode DisplayMode { get; set; }

        public bool Enabled { get; set; }

        public string ID { get; set; }

        public int MinLength { get; set; }
    }
}

