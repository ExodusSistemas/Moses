namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal static class GridUtil
    {
        internal static string GetAttachEditorsFunction(Moses.Web.Mvc.Controls.GridControl grid, string editorType, string editorControlID)
        {
            GetListOfColumns(grid);
            StringBuilder builder = new StringBuilder();
            builder.Append("function(el) {");
            builder.Append("setTimeout(function() {");
            builder.AppendFormat("var ec = '{0}';", editorControlID);
            if (editorType == "datepicker")
            {
                builder.Append("if (typeof(jQuery(el).datepicker) !== 'function')");
                builder.Append("alert('JQDatePicker javascript not present on the page. Please, include jquery.jqDatePicker.min.js');");
                builder.Append("jQuery(el).datepicker( eval(ec + '_dpid') );");
            }
            if (editorType == "autocomplete")
            {
                builder.Append("if (typeof(jQuery(el).autocomplete) !== 'function')");
                builder.Append("alert('JQAutoComplete javascript not present on the page. Please, include jquery.jqAutoComplete.min.js');");
                builder.Append("jQuery(el).autocomplete( eval(ec + '_acid') );");
            }
            builder.Append("},200);");
            builder.Append("}");
            return builder.ToString();
        }

        internal static List<string> GetListOfColumns(Moses.Web.Mvc.Controls.GridControl grid)
        {
            List<string> result = new List<string>();
            grid.Columns.FindAll(delegate (Moses.Web.Mvc.Controls.GridControlColumn c) {
                return true;
            }).ForEach(delegate (Moses.Web.Mvc.Controls.GridControlColumn c) {
                Moses.Web.Mvc.Controls.Guard.IsNotNullOrEmpty(c.EditorControlID, "GridControlColumn.EditorControlID", "must be set to the ID of the editing control control if EditType = DatePicker or EditType = AutoComplete");
                result.Add(c.EditType.ToString().ToLower() + ":" + c.DataField);
            });
            return result;
        }

        

        internal static List<string> GetListOfSearchColumns(Moses.Web.Mvc.Controls.GridControl grid)
        {
            List<string> result = new List<string>();
            grid.Columns.FindAll(delegate (Moses.Web.Mvc.Controls.GridControlColumn c) {
                if (c.SearchType != Moses.Web.Mvc.Controls.SearchType.DatePicker)
                {
                    return c.SearchType == Moses.Web.Mvc.Controls.SearchType.AutoComplete;
                }
                return true;
            }).ForEach(delegate (Moses.Web.Mvc.Controls.GridControlColumn c) {
                Moses.Web.Mvc.Controls.Guard.IsNotNullOrEmpty(c.SearchControlID, "JQGridColumn.SearchControlID", "must be set to the ID of the searching control control if SearchType = DatePicker or SearchType = AutoComplete");
                result.Add(c.SearchType.ToString().ToLower() + ":" + c.DataField);
            });
            return result;
        }

        internal static List<string> GetListOfSearchEditors(Moses.Web.Mvc.Controls.GridControl grid)
        {
            List<string> result = new List<string>();
            grid.Columns.FindAll(delegate (Moses.Web.Mvc.Controls.GridControlColumn c) {
                if (c.SearchType != Moses.Web.Mvc.Controls.SearchType.DatePicker)
                {
                    return c.SearchType == Moses.Web.Mvc.Controls.SearchType.AutoComplete;
                }
                return true;
            }).ForEach(delegate (Moses.Web.Mvc.Controls.GridControlColumn c) {
                Moses.Web.Mvc.Controls.Guard.IsNotNullOrEmpty(c.SearchControlID, "JQGridColumn.SearchControlID", "must be set to the ID of the searching control control if SearchType = DatePicker or SearchType = AutoComplete");
                result.Add(c.SearchControlID);
            });
            return result;
        }
    }
}

