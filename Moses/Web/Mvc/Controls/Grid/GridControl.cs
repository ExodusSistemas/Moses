namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Grid Extension based on free JQGrid 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GridControl<T> : GridControl
        where T : class, new()
    {
        public GridControl()
        {
            this.SetDefaultConfigs();
        }

        public T ColumnBuilder { get; set; }

        public void SetDefaultConfigs(string initialSortColumn = "Id", bool readOnly = false, bool hasDetails = true, int pageSize = 50)
        {
            this.ID = string.Format("FnGrid_{0}", DateTime.Now.Ticks.ToString());
            //grid.ClientSideEvents.GridInitialized = "$grid.OnCreateInstance";
            this.ClientSideEvents.SubGridRowExpanded = hasDetails ? "$grid.GetRowEvent('OnGridRowExpanded')" : null;
            this.ClientSideEvents.RowSelect = "$grid.GetRowEvent('OnGridRowSelect')";
            this.MultiSelect = readOnly ? false : true;
            this.SortSettings.InitialSortColumn = initialSortColumn;
            this.SortSettings.InitialSortDirection = Moses.Web.Mvc.Controls.SortDirection.Asc;
            this.PagerSettings.PageSize = pageSize;
            this.PagerSettings.ScrollBarPaging = false;
            this.ToolBarSettings.ToolBarPosition = ToolBarPosition.Hidden;
            this.HierarchySettings.HierarchyMode = hasDetails ? HierarchyMode.Parent : HierarchyMode.None;
            this.AppearanceSettings.HighlightRowsOnHover = true;
            this.AppearanceSettings.ShowRowNumbers = true;
            this.PagerSettings.NoRowsMessage = Moses.Web.Configuration.ApplicationConfiguration.NoRowsMessage;

            this.AutoWidth = true;
            this.Height = System.Web.UI.WebControls.Unit.Percentage(100);
            this.HierarchySettings.HierarchyMode = HierarchyMode.None;
        }
    }


    public class GridControl
    {
        private EventHandlerList _events;
        private static readonly object EventDataResolved = new object();

        public event Moses.Web.Mvc.Controls.JQGridDataResolvedEventHandler DataResolved
        {
            add
            {
                this.Events.AddHandler(EventDataResolved, value);
            }
            remove
            {
                this.Events.RemoveHandler(EventDataResolved, value);
            }
        }

        public GridControl()
        {
            this.AutoWidth = false;
            this.ShrinkToFit = true;
            this.SearchToolBarSettings = new Moses.Web.Mvc.Controls.SearchToolBarSettings();
            this.PagerSettings = new Moses.Web.Mvc.Controls.PagerSettings();
            this.ToolBarSettings = new Moses.Web.Mvc.Controls.ToolBarSettings();
            this.SortSettings = new Moses.Web.Mvc.Controls.SortSettings();
            this.AppearanceSettings = new Moses.Web.Mvc.Controls.AppearanceSettings();
            this.HierarchySettings = new Moses.Web.Mvc.Controls.HierarchySettings();
            this.GroupSettings = new Moses.Web.Mvc.Controls.GroupSettings();
            this.ClientSideEvents = new Moses.Web.Mvc.Controls.ClientSideEvents();
            this.Columns = new List<Moses.Web.Mvc.Controls.GridControlColumn>();
            this.DataUrl = "";
            this.EditUrl = "";
            this.ColumnReordering = false;
            this.RenderingMode = Moses.Web.Mvc.Controls.RenderingMode.Default;
            this.MultiSelect = false;
            this.MultiSelectMode = Moses.Web.Mvc.Controls.MultiSelectMode.SelectOnRowClick;
            this.MultiSelectKey = Moses.Web.Mvc.Controls.MultiSelectKey.None;
            this.Width = Unit.Empty;
            this.Height = Unit.Empty;
        }

        public JsonResult DataBind()
        {
            if (this.AjaxCallBackMode == Moses.Web.Mvc.Controls.AjaxCallBackMode.RequestData)
            {
            }
            return this.GetJsonResponse();
        }

        public JsonResult DataBind(object dataSource)
        {
            this.DataSource = dataSource;
            return this.DataBind();
        }

        private void DoExportToExcel(object dataSource, string fileName)
        {
            GridView view = new GridView {
                AutoGenerateColumns = false
            };
            foreach (Moses.Web.Mvc.Controls.GridControlColumn column in this.Columns)
            {
                BoundField field = new BoundField {
                    DataField = column.DataField
                };
                string str = string.IsNullOrEmpty(column.HeaderText) ? column.DataField : column.HeaderText;
                field.HeaderText = str;
                field.DataFormatString = column.DataFormatString;
                field.FooterText = column.FooterValue;
                view.Columns.Add(field);
            }
            view.DataSource = dataSource;
            view.DataBind();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            HttpContext.Current.Response.ContentType = "application/excel";
            StringWriter writer = new StringWriter();
            HtmlTextWriter writer2 = new HtmlTextWriter(writer);
            view.RenderControl(writer2);
            HttpContext.Current.Response.Write(writer.ToString());
            HttpContext.Current.Response.End();
        }

        private void DoExportToExcelWithState(object dataSource, string fileName, Moses.Web.Mvc.Controls.JQGridState gridState)
        {
            IQueryable queryable;
            if (!gridState.CurrentPageOnly)
            {
                gridState.QueryString["page"] = "1";
                gridState.QueryString["rows"] = "1000000";
            }
            this.FilterDataSource(dataSource, gridState.QueryString, out queryable);
            this.ExportToExcel(queryable, fileName);
        }

        public void ExportToExcel(object dataSource)
        {
            this.DoExportToExcel(dataSource, "GridExcelExport.xls");
        }

        public void ExportToExcel(object dataSource, string fileName)
        {
            this.DoExportToExcel(dataSource, fileName);
        }

        public void ExportToExcel(object dataSource, Moses.Web.Mvc.Controls.JQGridState gridState)
        {
            this.DoExportToExcelWithState(dataSource, "GridExcelExport.xls", gridState);
        }

        public void ExportToExcel(object dataSource, string fileName, Moses.Web.Mvc.Controls.JQGridState gridState)
        {
            this.DoExportToExcelWithState(dataSource, fileName, gridState);
        }

        private JsonResult FilterDataSource(object dataSource, NameValueCollection queryString, out IQueryable iqueryable)
        {
            iqueryable = dataSource as IQueryable;
            Moses.Web.Mvc.Controls.Guard.IsNotNull(iqueryable, "DataSource", "should implement the IQueryable interface.");
            int currentPage = Convert.ToInt32(queryString["page"]);
            int count = Convert.ToInt32(queryString["rows"]);
            string primaryKeyField = queryString["sidx"];
            string str2 = queryString["sord"];
            string text1 = queryString["parentRowID"];
            string str3 = queryString["_search"];
            string str4 = queryString["filters"];
            string str5 = queryString["searchField"];
            string searchString = queryString["searchString"];
            string searchOper = queryString["searchOper"];
            this.PagerSettings.CurrentPage = currentPage;
            this.PagerSettings.PageSize = count;
            if (!string.IsNullOrEmpty(str3) && (str3 != "false"))
            {
                try
                {
                    if (string.IsNullOrEmpty(str4) && !string.IsNullOrEmpty(str5))
                    {
                        iqueryable = iqueryable.Where(Moses.Web.Mvc.Controls.Util.GetWhereClause(this, str5, searchString, searchOper), new object[0]);
                    }
                    else if (!string.IsNullOrEmpty(str4))
                    {
                        iqueryable = iqueryable.Where(Moses.Web.Mvc.Controls.Util.GetWhereClause(this, str4), new object[0]);
                    }
                    else if (this.ToolBarSettings.ShowSearchToolBar || (str3 == "true"))
                    {
                        iqueryable = iqueryable.Where(Moses.Web.Mvc.Controls.Util.GetWhereClause(this, queryString), new object[0]);
                    }
                }
                catch (Moses.Web.Mvc.Controls.DataTypeNotSetException exception)
                {
                    throw exception;
                }
                catch (Exception)
                {
                    JsonResult result = new JsonResult();
                    result.Data =new object();
                    result.JsonRequestBehavior = 0;
                    return result;
                }
            }
            int totalRowCount = 0;
            int totalPagesCount = 0;
            totalRowCount = iqueryable.Count();
            totalPagesCount = (int) Math.Ceiling((double) (((float) totalRowCount) / ((float) count)));
            if (string.IsNullOrEmpty(primaryKeyField))
            {
                if (this.Columns.Count == 0)
                {
                    throw new Exception("JQGrid must have at least one column defined.");
                }
                primaryKeyField = Moses.Web.Mvc.Controls.Util.GetPrimaryKeyField(this);
                str2 = "asc";
            }
            if (!string.IsNullOrEmpty(primaryKeyField))
            {
                string ordering = "";
                if (this.GroupSettings.GroupFields.Count > 0)
                {
                    string str9 = primaryKeyField.Split(new char[] { ' ' })[0];
                    string str10 = primaryKeyField.Split(new char[] { ' ' })[1].Split(new char[] { ',' })[0];
                    primaryKeyField = primaryKeyField.Split(new char[] { ',' })[1];
                    ordering = str9 + " " + str10;
                }
                if ((primaryKeyField != null) && (primaryKeyField == " "))
                {
                    primaryKeyField = "";
                }
                if (!string.IsNullOrEmpty(primaryKeyField))
                {
                    if ((this.GroupSettings.GroupFields.Count > 0) && !ordering.EndsWith(","))
                    {
                        ordering = ordering + ",";
                    }
                    ordering = ordering + primaryKeyField + " " + str2;
                }
                iqueryable = iqueryable.OrderBy(ordering, new object[0]);
            }
            iqueryable = iqueryable.Skip(((currentPage - 1) * count)).Take(count);
            DataTable dt = iqueryable.ToDataTable(this);
            this.OnDataResolved(new Moses.Web.Mvc.Controls.JQGridDataResolvedEventArgs(this, iqueryable, this.DataSource as IQueryable));
            Moses.Web.Mvc.Controls.JsonResponse response = new Moses.Web.Mvc.Controls.JsonResponse(currentPage, totalPagesCount, totalRowCount, count, dt.Rows.Count, Moses.Web.Mvc.Controls.Util.GetFooterInfo(this));
            return Moses.Web.Mvc.Controls.Util.ConvertToJson(response, this, dt);
        }

        private JsonResult GetJsonResponse()
        {
            IQueryable queryable;
            Moses.Web.Mvc.Controls.Guard.IsNotNull(this.DataSource, "DataSource");
            return this.FilterDataSource(this.DataSource, HttpContext.Current.Request.QueryString, out queryable);
        }

        public Moses.Web.Mvc.Controls.JQGridState GetState()
        {
            NameValueCollection values = new NameValueCollection();
            foreach (string str in HttpContext.Current.Request.QueryString.Keys)
            {
                values.Add(str, HttpContext.Current.Request.QueryString[str]);
            }
            return new Moses.Web.Mvc.Controls.JQGridState { QueryString = values };
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public Moses.Web.Mvc.Controls.JQGridState GetState(bool currentPageOnly)
        {
            NameValueCollection values = new NameValueCollection();
            foreach (string str in HttpContext.Current.Request.QueryString.Keys)
            {
                values.Add(str, HttpContext.Current.Request.QueryString[str]);
            }
            return new Moses.Web.Mvc.Controls.JQGridState { 
                QueryString = values,
                CurrentPageOnly = currentPageOnly
            };
        }

        protected internal virtual void OnDataResolved(Moses.Web.Mvc.Controls.JQGridDataResolvedEventArgs e)
        {
            Moses.Web.Mvc.Controls.JQGridDataResolvedEventHandler handler = (Moses.Web.Mvc.Controls.JQGridDataResolvedEventHandler) this.Events[EventDataResolved];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public ActionResult ShowEditValidationMessage(string errorMessage)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.StatusCode = 500;
            HttpContext.Current.Response.StatusDescription = errorMessage;
            ContentResult result = new ContentResult();
            result.Content = errorMessage;
            return result;
        }

        public Moses.Web.Mvc.Controls.AjaxCallBackMode AjaxCallBackMode
        {
            get
            {
                string str4;
                string str = HttpContext.Current.Request.Form["oper"];
                string str2 = HttpContext.Current.Request.QueryString["editMode"];
                string str3 = HttpContext.Current.Request.QueryString["_search"];
                Moses.Web.Mvc.Controls.AjaxCallBackMode requestData = Moses.Web.Mvc.Controls.AjaxCallBackMode.RequestData;
                if (!string.IsNullOrEmpty(str) && ((str4 = str) != null))
                {
                    if (str4 == "add")
                    {
                        return Moses.Web.Mvc.Controls.AjaxCallBackMode.AddRow;
                    }
                    if (str4 == "edit")
                    {
                        return Moses.Web.Mvc.Controls.AjaxCallBackMode.EditRow;
                    }
                    if (str4 == "del")
                    {
                        return Moses.Web.Mvc.Controls.AjaxCallBackMode.DeleteRow;
                    }
                }
                if (!string.IsNullOrEmpty(str2))
                {
                    requestData = Moses.Web.Mvc.Controls.AjaxCallBackMode.EditRow;
                }
                if (!string.IsNullOrEmpty(str3) && Convert.ToBoolean(str3))
                {
                    requestData = Moses.Web.Mvc.Controls.AjaxCallBackMode.Search;
                }
                return requestData;
            }
        }

        public Moses.Web.Mvc.Controls.AppearanceSettings AppearanceSettings { get; set; }

        public bool AutoWidth { get; set; }

        public Moses.Web.Mvc.Controls.ClientSideEvents ClientSideEvents { get; set; }

        public bool ColumnReordering { get; set; }

        public List<Moses.Web.Mvc.Controls.GridControlColumn> Columns { get; set; }

        public object DataSource { get; set; }

        public string DataUrl { get; set; }

        public string EditUrl { get; set; }

        private EventHandlerList Events
        {
            get
            {
                if (this._events == null)
                {
                    this._events = new EventHandlerList();
                }
                return this._events;
            }
        }

        public Moses.Web.Mvc.Controls.GroupSettings GroupSettings { get; set; }

        public Unit Height { get; set; }

        public Moses.Web.Mvc.Controls.HierarchySettings HierarchySettings { get; set; }

        public string ID { get; set; }

        public bool MultiSelect { get; set; }

        public Moses.Web.Mvc.Controls.MultiSelectKey MultiSelectKey { get; set; }

        public Moses.Web.Mvc.Controls.MultiSelectMode MultiSelectMode { get; set; }

        public Moses.Web.Mvc.Controls.PagerSettings PagerSettings { get; set; }

        public Moses.Web.Mvc.Controls.RenderingMode RenderingMode { get; set; }

        public Moses.Web.Mvc.Controls.SearchToolBarSettings SearchToolBarSettings { get; set; }

        internal bool ShowToolBar
        {
            get
            {
                if (((!this.ToolBarSettings.ShowAddButton && !this.ToolBarSettings.ShowDeleteButton) && (!this.ToolBarSettings.ShowEditButton && !this.ToolBarSettings.ShowRefreshButton)) && (!this.ToolBarSettings.ShowSearchButton && !this.ToolBarSettings.ShowViewRowDetailsButton))
                {
                    return (this.ToolBarSettings.CustomButtons.Count > 0);
                }
                return true;
            }
        }

        public bool ShrinkToFit { get; set; }

        public Moses.Web.Mvc.Controls.SortSettings SortSettings { get; set; }

        public Moses.Web.Mvc.Controls.ToolBarSettings ToolBarSettings { get; set; }

        public Unit Width { get; set; }
    }
}

