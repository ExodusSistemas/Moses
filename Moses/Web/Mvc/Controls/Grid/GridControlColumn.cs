namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Web;
    using System.Web.Mvc;

    public class GridControlColumn
    {
        

        public GridControlColumn()
        {
            this.Width = 150;
            this.Sortable = true;
            this.Resizable = true;
            this.PrimaryKey = false;
            this.SearchType = Moses.Web.Mvc.Controls.SearchType.TextBox;
            this.SearchControlID = "";
            this.SearchToolBarOperation = Moses.Web.Mvc.Controls.SearchOperation.Contains;
            this.SearchList = new List<SelectListItem>();
            this.SearchCaseSensitive = false;
            this.DataField = "";
            this.DataFormatString = "";
            this.HeaderText = "";
            this.TextAlign = Moses.Web.Mvc.Controls.TextAlign.Left;
            this.Visible = true;
            this.Searchable = true;
            this.HtmlEncode = true;
            this.HtmlEncodeFormatString = true;
            this.ConvertEmptyStringToNull = true;
            this.NullDisplayText = "";
            this.FooterValue = "";
            this.CssClass = "";
            this.GroupSummaryType = Moses.Web.Mvc.Controls.GroupSummaryType.None;
            this.GroupTemplate = "";
            this.Fixed = false;
        }

        internal virtual string FormatDataValue(object dataValue, bool encode)
        {
            if (this.IsNull(dataValue))
            {
                return this.NullDisplayText;
            }
            string s = dataValue.ToString();
            string dataFormatString = this.DataFormatString;
            int length = s.Length;
            if (!this.HtmlEncodeFormatString)
            {
                if ((length > 0) && encode)
                {
                    s = HttpUtility.HtmlEncode(s);
                }
                if ((length == 0) && this.ConvertEmptyStringToNull)
                {
                    return this.NullDisplayText;
                }
                if (dataFormatString.Length == 0)
                {
                    return s;
                }
                if (encode)
                {
                    return string.Format(CultureInfo.CurrentCulture, dataFormatString, new object[] { s });
                }
                return string.Format(CultureInfo.CurrentCulture, dataFormatString, new object[] { dataValue });
            }
            if ((length == 0) && this.ConvertEmptyStringToNull)
            {
                return this.NullDisplayText;
            }
            if (!string.IsNullOrEmpty(dataFormatString))
            {
                s = string.Format(CultureInfo.CurrentCulture, dataFormatString, new object[] { dataValue });
            }
            if (!string.IsNullOrEmpty(s) && encode)
            {
                s = HttpUtility.HtmlEncode(s);
            }
            return s;
        }

        internal bool IsNull(object value)
        {
            if ((value != null) && !Convert.IsDBNull(value))
            {
                return false;
            }
            return true;
        }

        public bool ConvertEmptyStringToNull { get; set; }

        public string CssClass { get; set; }

        public string DataField { get; set; }

        public string DataFormatString { get; set; }

        public EditorType EditType { get; set; }

        public Type DataType { get; set; }

        public bool Fixed { get; set; }

        public string FooterValue { get; set; }

        public Moses.Web.Mvc.Controls.JQGridColumnFormatter Formatter { get; set; }

        public Moses.Web.Mvc.Controls.GroupSummaryType GroupSummaryType { get; set; }

        public string GroupTemplate { get; set; }

        public string HeaderText { get; set; }

        public bool HtmlEncode { get; set; }

        public bool HtmlEncodeFormatString { get; set; }

        public string NullDisplayText { get; set; }

        public bool PrimaryKey { get; set; }

        public bool Resizable { get; set; }

        public bool Searchable { get; set; }

        public bool SearchCaseSensitive { get; set; }

        public string SearchControlID { get; set; }

        public List<SelectListItem> SearchList { get; set; }

        public Moses.Web.Mvc.Controls.SearchOperation SearchToolBarOperation { get; set; }

        public Moses.Web.Mvc.Controls.SearchType SearchType { get; set; }

        public bool Sortable { get; set; }

        public Moses.Web.Mvc.Controls.TextAlign TextAlign { get; set; }

        public bool Visible { get; set; }

        public int Width { get; set; }
        public string EditorControlID { get; internal set; }
    }
}

