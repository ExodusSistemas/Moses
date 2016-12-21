namespace Moses.Web.Mvc.Controls
{
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Web;
    using System.Web.Mvc;

    public class AutoCompleteControl<T>
    {
        public AutoCompleteControl()
        {
            this.AutoCompleteMode = Moses.Web.Mvc.Controls.AutoCompleteMode.BeginsWith;
            this.DataField = "";
            this.DataSource = null;
            this.Max = 150;
        }

        public JsonResult DataBind() => 
            this.GetJsonResponse();

        public JsonResult DataBind(IQueryable<T> dataSource)
        {
            this.DataSource = dataSource;
            return this.DataBind();
        }

        private JsonResult GetJsonResponse()
        {
            var dataSource = EvaluateQuery();
            JsonResult result2 = new JsonResult();
            result2.JsonRequestBehavior = 0;
            result2.Data = dataSource.ToListOfString(this);
            return result2;
        }

        public IQueryable<T> EvaluateQuery(){
            IQueryable<T> output = null;
            Moses.Web.Mvc.Controls.Guard.IsNotNull(this.DataSource, "DataSource");
            Moses.Web.Mvc.Controls.Guard.IsNotNull(this.DataSource, "DataSource", "should implement the IQueryable interface.");
            Moses.Web.Mvc.Controls.Guard.IsNotNullOrEmpty(this.DataField, "DataField", "should be set to the datafield (column) of the datasource to search in.");
            Moses.Web.Mvc.Controls.SearchOperation isEqualTo = Moses.Web.Mvc.Controls.SearchOperation.Contains;
            if (this.AutoCompleteMode == Moses.Web.Mvc.Controls.AutoCompleteMode.BeginsWith)
            {
                isEqualTo = Moses.Web.Mvc.Controls.SearchOperation.BeginsWith;
            }
            else
            {
                isEqualTo = Moses.Web.Mvc.Controls.SearchOperation.Contains;
            }
            if (!string.IsNullOrEmpty(this.Term))
            {
                Moses.Web.Mvc.Controls.Util.SearchArguments args = new Moses.Web.Mvc.Controls.Util.SearchArguments {
                    SearchColumn = this.DataField,
                    SearchOperation = isEqualTo,
                    SearchString = this.Term
                };
                output = this.DataSource.Where(Moses.Web.Mvc.Controls.Util.ConstructLinqFilterExpression(this, args), args.SearchString);
            }

            return output;
        }

        public Moses.Web.Mvc.Controls.AutoCompleteMode AutoCompleteMode { get; set; }

        public string DataField { get; set; }

        public IQueryable<T> DataSource { get; set; }

        public string Term { get; set; }
        
        public int Max { get; set; }

    }
}

