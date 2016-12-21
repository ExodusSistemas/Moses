namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    internal class JsonColModel
    {
        private Moses.Web.Mvc.Controls.GridControl _grid;
        private Hashtable _jsonValues;

        public JsonColModel(Moses.Web.Mvc.Controls.GridControl grid)
        {
            this._jsonValues = new Hashtable();
            this._grid = grid;
        }

        public JsonColModel(Moses.Web.Mvc.Controls.GridControlColumn column, Moses.Web.Mvc.Controls.GridControl grid) : this(grid)
        {
            this.FromColumn(column);
        }

        private void ApplyFormatterOptions(Moses.Web.Mvc.Controls.GridControlColumn column)
        {
            Hashtable hashtable = new Hashtable();
            if (column.Formatter != null)
            {
                Moses.Web.Mvc.Controls.JQGridColumnFormatter formatter = column.Formatter;
                if (formatter is Moses.Web.Mvc.Controls.LinkFormatter)
                {
                    Moses.Web.Mvc.Controls.LinkFormatter formatter2 = (Moses.Web.Mvc.Controls.LinkFormatter) formatter;
                    this._jsonValues["formatter"] = "link";
                    if (!string.IsNullOrEmpty(formatter2.Target))
                    {
                        hashtable["target"] = formatter2.Target;
                    }
                }
                if (formatter is Moses.Web.Mvc.Controls.EmailFormatter)
                {
                    this._jsonValues["formatter"] = "email";
                }
                if (formatter is Moses.Web.Mvc.Controls.IntegerFormatter)
                {
                    Moses.Web.Mvc.Controls.IntegerFormatter formatter3 = (Moses.Web.Mvc.Controls.IntegerFormatter) formatter;
                    this._jsonValues["formatter"] = "integer";
                    if (!string.IsNullOrEmpty(formatter3.ThousandsSeparator))
                    {
                        hashtable["thousandsSeparator"] = formatter3.ThousandsSeparator;
                    }
                    if (!string.IsNullOrEmpty(formatter3.DefaultValue))
                    {
                        hashtable["defaultValue"] = formatter3.DefaultValue;
                    }
                }
                if (formatter is Moses.Web.Mvc.Controls.NumberFormatter)
                {
                    Moses.Web.Mvc.Controls.NumberFormatter formatter4 = (Moses.Web.Mvc.Controls.NumberFormatter) formatter;
                    this._jsonValues["formatter"] = "integer";
                    if (!string.IsNullOrEmpty(formatter4.ThousandsSeparator))
                    {
                        hashtable["thousandsSeparator"] = formatter4.ThousandsSeparator;
                    }
                    if (!string.IsNullOrEmpty(formatter4.DefaultValue))
                    {
                        hashtable["defaultValue"] = formatter4.DefaultValue;
                    }
                    if (!string.IsNullOrEmpty(formatter4.DecimalSeparator))
                    {
                        hashtable["decimalSeparator"] = formatter4.DecimalSeparator;
                    }
                    if (formatter4.DecimalPlaces != -1)
                    {
                        hashtable["decimalPlaces"] = formatter4.DecimalPlaces;
                    }
                }
                if (formatter is Moses.Web.Mvc.Controls.CurrencyFormatter)
                {
                    Moses.Web.Mvc.Controls.CurrencyFormatter formatter5 = (Moses.Web.Mvc.Controls.CurrencyFormatter) formatter;
                    this._jsonValues["formatter"] = "currency";
                    if (!string.IsNullOrEmpty(formatter5.ThousandsSeparator))
                    {
                        hashtable["thousandsSeparator"] = formatter5.ThousandsSeparator;
                    }
                    if (!string.IsNullOrEmpty(formatter5.DefaultValue))
                    {
                        hashtable["defaultValue"] = formatter5.DefaultValue;
                    }
                    if (!string.IsNullOrEmpty(formatter5.DecimalSeparator))
                    {
                        hashtable["decimalSeparator"] = formatter5.DecimalSeparator;
                    }
                    if (formatter5.DecimalPlaces != -1)
                    {
                        hashtable["decimalPlaces"] = formatter5.DecimalPlaces;
                    }
                    if (!string.IsNullOrEmpty(formatter5.Prefix))
                    {
                        hashtable["prefix"] = formatter5.Prefix;
                    }
                    if (!string.IsNullOrEmpty(formatter5.Prefix))
                    {
                        hashtable["suffix"] = formatter5.Suffix;
                    }
                }
                if (formatter is Moses.Web.Mvc.Controls.CheckBoxFormatter)
                {
                    Moses.Web.Mvc.Controls.CheckBoxFormatter formatter6 = (Moses.Web.Mvc.Controls.CheckBoxFormatter) formatter;
                    this._jsonValues["formatter"] = "checkbox";
                    if (formatter6.Enabled)
                    {
                        hashtable["disabled"] = false;
                    }
                }
                if (formatter is Moses.Web.Mvc.Controls.CustomFormatter)
                {
                    Moses.Web.Mvc.Controls.CustomFormatter formatter7 = (Moses.Web.Mvc.Controls.CustomFormatter) formatter;
                    if (!string.IsNullOrEmpty(formatter7.FormatFunction))
                    {
                        this._jsonValues["formatter"] = formatter7.FormatFunction;
                    }
                    if (!string.IsNullOrEmpty(formatter7.UnFormatFunction))
                    {
                        this._jsonValues["unformat"] = formatter7.UnFormatFunction;
                    }
                }
            }
            if (hashtable.Count > 0)
            {
                this._jsonValues["formatoptions"] = hashtable;
            }
        }

        public void FromColumn(Moses.Web.Mvc.Controls.GridControlColumn column)
        {
            this._jsonValues["index"] = this._jsonValues["name"] = column.DataField;
            if (column.Width != 150)
            {
                this._jsonValues["width"] = column.Width;
            }
            if (!column.Sortable)
            {
                this._jsonValues["sortable"] = false;
            }
            if (column.PrimaryKey)
            {
                this._jsonValues["key"] = true;
            }
            if (!column.Visible)
            {
                this._jsonValues["hidden"] = true;
            }
            if (!column.Searchable)
            {
                this._jsonValues["search"] = false;
            }
            if (column.TextAlign != Moses.Web.Mvc.Controls.TextAlign.Left)
            {
                this._jsonValues["align"] = column.TextAlign.ToString().ToLower();
            }
            if (!column.Resizable)
            {
                this._jsonValues["resizable"] = false;
            }
            if (!string.IsNullOrEmpty(column.CssClass))
            {
                this._jsonValues["classes"] = column.CssClass;
            }
            if (column.Fixed)
            {
                this._jsonValues["fixed"] = true;
            }
            if (column.Formatter != null )
            {
                this.ApplyFormatterOptions(column);
            }
            switch (column.GroupSummaryType)
            {
                case Moses.Web.Mvc.Controls.GroupSummaryType.Min:
                    this._jsonValues["summaryType"] = "min";
                    break;

                case Moses.Web.Mvc.Controls.GroupSummaryType.Max:
                    this._jsonValues["summaryType"] = "max";
                    break;

                case Moses.Web.Mvc.Controls.GroupSummaryType.Sum:
                    this._jsonValues["summaryType"] = "sum";
                    break;

                case Moses.Web.Mvc.Controls.GroupSummaryType.Avg:
                    this._jsonValues["summaryType"] = "avg";
                    break;

                case Moses.Web.Mvc.Controls.GroupSummaryType.Count:
                    this._jsonValues["summaryType"] = "count";
                    break;
            }
            if (!string.IsNullOrEmpty(column.GroupTemplate))
            {
                this._jsonValues["summaryTpl"] = column.GroupTemplate;
            }
            if (column.Searchable)
            {
                Hashtable hashtable = new Hashtable();
                if (column.SearchType == Moses.Web.Mvc.Controls.SearchType.DropDown)
                {
                    this._jsonValues["stype"] = "select";
                }
                if (!column.Visible)
                {
                    hashtable["searchhidden"] = true;
                }
                if (column.SearchList.Count<SelectListItem>() > 0)
                {
                    StringBuilder builder = new StringBuilder();
                    int num = 0;
                    foreach (SelectListItem item in column.SearchList)
                    {
                        builder.AppendFormat("{0}:{1}", item.Value, item.Text);
                        num++;
                        if (num < column.SearchList.Count<SelectListItem>())
                        {
                            builder.Append(";");
                        }
                        if (item.Selected)
                        {
                            hashtable["defaultValue"] = item.Value;
                        }
                    }
                    hashtable["value"] = builder.ToString();
                }
                if ((column.SearchType == Moses.Web.Mvc.Controls.SearchType.DatePicker) || (column.SearchType == Moses.Web.Mvc.Controls.SearchType.AutoComplete))
                {
                    hashtable["dataInit"] = "attachSearchControlsScript" + column.DataField;
                }
                this._jsonValues["searchoptions"] = hashtable;
            }
        }

        private string GetEditType(Moses.Web.Mvc.Controls.EditType type)
        {
            switch (type)
            {
                case Moses.Web.Mvc.Controls.EditType.CheckBox:
                    return "checkbox";

                case Moses.Web.Mvc.Controls.EditType.Custom:
                    return "custom";

                case Moses.Web.Mvc.Controls.EditType.DropDown:
                    return "select";

                case Moses.Web.Mvc.Controls.EditType.Password:
                    return "password";

                case Moses.Web.Mvc.Controls.EditType.TextArea:
                    return "textarea";

                case Moses.Web.Mvc.Controls.EditType.TextBox:
                    return "text";
            }
            return "text";
        }

        public static string RemoveQuotesForJavaScriptMethods(string input, Moses.Web.Mvc.Controls.GridControl grid)
        {
            string str = input;
            foreach (Moses.Web.Mvc.Controls.GridControlColumn column in grid.Columns)
            {
                if (column.Formatter != null)
                {
                    Moses.Web.Mvc.Controls.JQGridColumnFormatter formatter = column.Formatter;
                    if (formatter is Moses.Web.Mvc.Controls.CustomFormatter)
                    {
                        Moses.Web.Mvc.Controls.CustomFormatter formatter2 = (Moses.Web.Mvc.Controls.CustomFormatter) formatter;
                        string oldValue = $"\"formatter\":\"{formatter2.FormatFunction}\"";
                        string newValue = $"\"formatter\":{formatter2.FormatFunction}";
                        str = str.Replace(oldValue, newValue);
                        oldValue = $"\"unformat\":\"{formatter2.UnFormatFunction}\"";
                        newValue = $"\"unformat\":{formatter2.UnFormatFunction}";
                        str = str.Replace(oldValue, newValue);
                    }
                }
                
                if ((column.Searchable && (column.SearchType == Moses.Web.Mvc.Controls.SearchType.DatePicker)) || (column.SearchType == Moses.Web.Mvc.Controls.SearchType.AutoComplete))
                {
                    string str9 = Moses.Web.Mvc.Controls.GridUtil.GetAttachEditorsFunction(grid, column.SearchType.ToString().ToLower(), column.SearchControlID);
                    str = str.Replace(string.Concat(new object[] { '"', "attachSearchControlsScript", column.DataField, '"' }), str9);
                    str = str.Replace('"' + "dataInit" + '"', "dataInit");
                }
            }
            return str;
        }

        public Hashtable JsonValues
        {
            get
            {
                return this._jsonValues;
            }
            set
            {
                this._jsonValues = value;
            }
        }
    }
}

