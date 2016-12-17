namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Collections;
    using System.Text;
    using System.Web.Script.Serialization;

    internal class GridRenderer
    {

        private string GetChildSubGridJavaScript(Moses.Web.Mvc.Controls.GridControl grid)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<script type='text/javascript'>\n");
            builder.AppendFormat("function showSubGrid_{0}(subgrid_id, row_id, message, suffix) {{", grid.ID);
            builder.Append("var subgrid_table_id, pager_id;\r\n\t\t                subgrid_table_id = subgrid_id+'_t';\r\n\t\t                pager_id = 'p_'+ subgrid_table_id;\r\n                        if (suffix) { subgrid_table_id += suffix; pager_id += suffix;  }\r\n                        if (message) jQuery('#'+subgrid_id).append(message);                        \r\n\t\t                jQuery('#'+subgrid_id).append('<table id=' + subgrid_table_id + ' class=scroll></table><div id=' + pager_id + ' class=scroll></div>');\r\n                ");
            builder.Append(this.GetStartupOptions(grid, true));
            builder.Append("}");
            builder.Append("</script>");
            return builder.ToString();
        }

        private string GetColModel(Moses.Web.Mvc.Controls.GridControl grid)
        {
            Hashtable[] hashtableArray = new Hashtable[grid.Columns.Count];
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                Moses.Web.Mvc.Controls.JsonColModel model = new Moses.Web.Mvc.Controls.JsonColModel(grid.Columns[i], grid);
                hashtableArray[i] = model.JsonValues;
            }
            return Moses.Web.Mvc.Controls.JsonColModel.RemoveQuotesForJavaScriptMethods(new JavaScriptSerializer().Serialize(hashtableArray), grid);
        }

        private string GetColNames(Moses.Web.Mvc.Controls.GridControl grid)
        {
            string[] strArray = new string[grid.Columns.Count];
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                Moses.Web.Mvc.Controls.GridControlColumn column = grid.Columns[i];
                strArray[i] = string.IsNullOrEmpty(column.HeaderText) ? column.DataField : column.HeaderText;
            }
            return new JavaScriptSerializer().Serialize(strArray);
        }

        private string GetJQuerySubmit(Moses.Web.Mvc.Controls.GridControl grid)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("\r\n                        var _theForm = document.getElementsByTagName('FORM')[0];\r\n                        jQuery(_theForm).submit( function() \r\n                        {{  \r\n                            jQuery('#{0}').attr('value', jQuery('#{1}').getGridParam('selrow'));                            \r\n                        }});\r\n                       ", grid.ID + "_SelectedRow", grid.ID, grid.ID + "_CurrentPage");
            return builder.ToString();
        }

        private string GetLoadErrorHandler()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\n");
            builder.Append("function jqGrid_aspnet_loadErrorHandler(xht, st, handler) {");
            builder.Append("jQuery(document.body).css('font-size','100%'); jQuery(document.body).html(xht.responseText);");
            builder.Append("}");
            return builder.ToString();
        }

        private string GetMultiKeyString(Moses.Web.Mvc.Controls.MultiSelectKey key)
        {
            switch (key)
            {
                case Moses.Web.Mvc.Controls.MultiSelectKey.Shift:
                    return "shiftKey";

                case Moses.Web.Mvc.Controls.MultiSelectKey.Ctrl:
                    return "ctrlKey";

                case Moses.Web.Mvc.Controls.MultiSelectKey.Alt:
                    return "altKey";
            }
            throw new Exception("Should not be here.");
        }

        private string GetStartupJavascript(Moses.Web.Mvc.Controls.GridControl grid, bool subgrid)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<script type='text/javascript'>\n");
            builder.Append("jQuery(document).ready(function() {");
            builder.Append(this.GetStartupOptions(grid, subgrid));
            builder.Append("});");
            builder.Append("</script>");
            return builder.ToString();
        }

        private string GetStartupOptions(Moses.Web.Mvc.Controls.GridControl grid, bool subGrid)
        {
            StringBuilder builder = new StringBuilder();
            string str = subGrid ? "jQuery('#' + subgrid_table_id)" : $"jQuery('#{grid.ID}')";
            string str2 = subGrid ? "jQuery('#' + pager_id)" : $"jQuery('#{(grid.ID + "_pager")}')";
            string pagerSelectorID = subGrid ? "'#' + pager_id" : $"'#{(grid.ID + "_pager")}'";
            string str4 = subGrid ? "&parentRowID=' + row_id + '" : string.Empty;
            string str5 = (grid.DataUrl.IndexOf("?") > 0) ? "&" : "?";
            string str6 = (grid.EditUrl.IndexOf("?") > 0) ? "&" : "?";
            string str7 = $"{grid.DataUrl}{str5}jqGridID={grid.ID}{str4}";
            string str8 = $"{grid.EditUrl}{str6}jqGridID={grid.ID}&editMode=1{str4}";
            builder.AppendFormat("{0}.jqGrid({{", str);
            builder.AppendFormat("url: '{0}'", str7);
            builder.AppendFormat(",editurl: '{0}'", str8);
            builder.AppendFormat(",mtype: 'GET'", new object[0]);
            builder.AppendFormat(",datatype: 'json'", new object[0]);
            builder.AppendFormat(",page: {0}", grid.PagerSettings.CurrentPage);
            builder.AppendFormat(",colNames: {0}", this.GetColNames(grid));
            builder.AppendFormat(",colModel: {0}", this.GetColModel(grid));
            builder.AppendFormat(",viewrecords: true", new object[0]);
            builder.AppendFormat(",scrollrows: false", new object[0]);
            builder.AppendFormat(",prmNames: {{ id: \"{0}\" }}", Moses.Web.Mvc.Controls.Util.GetPrimaryKeyField(grid));
            if (grid.AppearanceSettings.ShowFooter)
            {
                builder.Append(",footerrow: true");
                builder.Append(",userDataOnFooter: true");
            }
            if (!grid.AppearanceSettings.ShrinkToFit)
            {
                builder.Append(",shrinkToFit: false");
            }
            if (grid.ColumnReordering)
            {
                builder.Append(",sortable: true");
            }
            if (grid.AppearanceSettings.ScrollBarOffset != 0x12)
            {
                builder.AppendFormat(",scrollOffset: {0}", grid.AppearanceSettings.ScrollBarOffset);
            }
            if (grid.AppearanceSettings.RightToLeft)
            {
                builder.Append(",direction: 'rtl'");
            }
            if (grid.AutoWidth)
            {
                builder.Append(",autowidth: true");
            }
            if (!grid.ShrinkToFit)
            {
                builder.Append(",shrinkToFit: false");
            }
            if ((grid.ToolBarSettings.ToolBarPosition == Moses.Web.Mvc.Controls.ToolBarPosition.Bottom) || (grid.ToolBarSettings.ToolBarPosition == Moses.Web.Mvc.Controls.ToolBarPosition.TopAndBottom))
            {
                builder.AppendFormat(",pager: {0}", str2);
            }
            if ((grid.ToolBarSettings.ToolBarPosition == Moses.Web.Mvc.Controls.ToolBarPosition.Top) || (grid.ToolBarSettings.ToolBarPosition == Moses.Web.Mvc.Controls.ToolBarPosition.TopAndBottom))
            {
                builder.Append(",toppager: true");
            }
            if (grid.RenderingMode == Moses.Web.Mvc.Controls.RenderingMode.Optimized)
            {
                if (grid.HierarchySettings.HierarchyMode != Moses.Web.Mvc.Controls.HierarchyMode.None)
                {
                    throw new Exception("Optimized rendering is not compatible with hierarchy.");
                }
                builder.Append(",gridview: true");
            }
            if ((grid.HierarchySettings.HierarchyMode == Moses.Web.Mvc.Controls.HierarchyMode.Parent) || (grid.HierarchySettings.HierarchyMode == Moses.Web.Mvc.Controls.HierarchyMode.ParentAndChild))
            {
                builder.Append(",subGrid: true");
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.SubGridRowExpanded))
            {
                builder.AppendFormat(",subGridRowExpanded: {0}", grid.ClientSideEvents.SubGridRowExpanded);
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.ServerError))
            {
                builder.AppendFormat(",errorCell: {0}", grid.ClientSideEvents.ServerError);
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.RowSelect))
            {
                builder.AppendFormat(",onSelectRow: {0}", grid.ClientSideEvents.RowSelect);
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.ColumnSort))
            {
                builder.AppendFormat(",onSortCol: {0}", grid.ClientSideEvents.ColumnSort);
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.RowDoubleClick))
            {
                builder.AppendFormat(",ondblClickRow: {0}", grid.ClientSideEvents.RowDoubleClick);
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.RowRightClick))
            {
                builder.AppendFormat(",onRightClickRow: {0}", grid.ClientSideEvents.RowRightClick);
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.LoadDataError))
            {
                builder.AppendFormat(",loadError: {0}", grid.ClientSideEvents.LoadDataError);
            }
            else
            {
                builder.AppendFormat(",loadError: {0}", "jqGrid_aspnet_loadErrorHandler");
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.GridInitialized))
            {
                builder.AppendFormat(",gridComplete: {0}", grid.ClientSideEvents.GridInitialized);
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.BeforeAjaxRequest))
            {
                builder.AppendFormat(",beforeRequest: {0}", grid.ClientSideEvents.BeforeAjaxRequest);
            }
            if (!grid.AppearanceSettings.HighlightRowsOnHover)
            {
                builder.Append(",hoverrows: false");
            }
            if (grid.AppearanceSettings.AlternateRowBackground)
            {
                builder.Append(",altRows: true");
            }
            if (grid.AppearanceSettings.ShowRowNumbers)
            {
                builder.Append(",rownumbers: true");
            }
            if (grid.AppearanceSettings.RowNumbersColumnWidth != 0x19)
            {
                builder.AppendFormat(",rownumWidth: {0}", grid.AppearanceSettings.RowNumbersColumnWidth.ToString());
            }
            if (grid.PagerSettings.ScrollBarPaging)
            {
                builder.AppendFormat(",scroll: 1", new object[0]);
            }
            builder.AppendFormat(",rowNum: {0}", grid.PagerSettings.PageSize.ToString());
            builder.AppendFormat(",rowList: {0}", grid.PagerSettings.PageSizeOptions.ToString());
            if (!string.IsNullOrEmpty(grid.PagerSettings.NoRowsMessage))
            {
                builder.AppendFormat(",emptyrecords: '{0}'", grid.PagerSettings.NoRowsMessage.ToString());
            }
            builder.AppendFormat(",jsonreader: {{ id: \"{0}\" }}", grid.Columns[Moses.Web.Mvc.Controls.Util.GetPrimaryKeyIndex(grid)].DataField);
            if (!string.IsNullOrEmpty(grid.SortSettings.InitialSortColumn))
            {
                builder.AppendFormat(",sortname: '{0}'", grid.SortSettings.InitialSortColumn);
            }
            builder.AppendFormat(",sortorder: '{0}'", grid.SortSettings.InitialSortDirection.ToString().ToLower());
            if (grid.MultiSelect)
            {
                builder.Append(",multiselect: true");
                if (grid.MultiSelectMode == Moses.Web.Mvc.Controls.MultiSelectMode.SelectOnCheckBoxClickOnly)
                {
                    builder.AppendFormat(",multiboxonly: true", grid.MultiSelect.ToString().ToLower());
                }
                if (grid.MultiSelectKey != Moses.Web.Mvc.Controls.MultiSelectKey.None)
                {
                    builder.AppendFormat(",multikey: '{0}'", this.GetMultiKeyString(grid.MultiSelectKey));
                }
            }
            if (!string.IsNullOrEmpty(grid.AppearanceSettings.Caption))
            {
                builder.AppendFormat(",caption: '{0}'", grid.AppearanceSettings.Caption);
            }
            if (!grid.Width.IsEmpty)
            {
                builder.AppendFormat(",width: '{0}'", grid.Width.ToString().Replace("px", ""));
            }
            if (!grid.Height.IsEmpty)
            {
                builder.AppendFormat(",height: '{0}'", grid.Height.ToString().Replace("px", ""));
            }
            if (grid.GroupSettings.GroupFields.Count > 0)
            {
                builder.Append(",grouping:true");
                builder.Append(",groupingView: {");
                builder.AppendFormat("groupField: ['{0}']", grid.GroupSettings.GroupFields[0].DataField);
                builder.AppendFormat(",groupColumnShow: [{0}]", grid.GroupSettings.GroupFields[0].ShowGroupColumn.ToString().ToLower());
                builder.AppendFormat(",groupText: ['{0}']", grid.GroupSettings.GroupFields[0].HeaderText);
                builder.AppendFormat(",groupOrder: ['{0}']", grid.GroupSettings.GroupFields[0].GroupSortDirection.ToString().ToLower());
                builder.AppendFormat(",groupSummary: [{0}]", grid.GroupSettings.GroupFields[0].ShowGroupSummary.ToString().ToLower());
                builder.AppendFormat(",groupCollapse: {0}", grid.GroupSettings.CollapseGroups.ToString().ToLower());
                builder.AppendFormat(",groupDataSorted: true", new object[0]);
                builder.Append("}");
            }
            builder.AppendFormat(",viewsortcols: [{0},'{1}',{2}]", "false", grid.SortSettings.SortIconsPosition.ToString().ToLower(), (grid.SortSettings.SortAction == Moses.Web.Mvc.Controls.SortAction.ClickOnHeader) ? "true" : "false");
            builder.AppendFormat("}})\r", new object[0]);
            builder.Append(this.GetToolBarOptions(grid, subGrid, pagerSelectorID));
            if (!grid.PagerSettings.ScrollBarPaging)
            {
                builder.AppendFormat(".bindKeys()", new object[0]);
            }
            builder.Append(";");
            builder.Append(this.GetLoadErrorHandler());
            if (grid.ToolBarSettings.ShowSearchToolBar)
            {
                builder.AppendFormat("{0}.filterToolbar({1});", str, new Moses.Web.Mvc.Controls.JsonSearchToolBar(grid).Process());
            }
            return builder.ToString();
        }

        private string GetToolBarOptions(Moses.Web.Mvc.Controls.GridControl grid, bool subGrid, string pagerSelectorID)
        {
            StringBuilder builder = new StringBuilder();
            if (!grid.ShowToolBar)
            {
                return string.Empty;
            }
            Moses.Web.Mvc.Controls.JsonToolBar bar = new Moses.Web.Mvc.Controls.JsonToolBar(grid.ToolBarSettings);
            if (!subGrid)
            {
                builder.AppendFormat(".navGrid('#{0}',{1},{2},{3},{4},{5} )", new object[] { grid.ID + "_pager", new JavaScriptSerializer().Serialize(bar), $"jQuery('#{grid.ID}').getGridParam('editDialogOptions')", $"jQuery('#{grid.ID}').getGridParam('addDialogOptions')", $"jQuery('#{grid.ID}').getGridParam('delDialogOptions')", $"jQuery('#{grid.ID}').getGridParam('searchDialogOptions')" });
            }
            else
            {
                builder.AppendFormat(".navGrid('#' + pager_id,{0},{1},{2},{3},{4} )", new object[] { new JavaScriptSerializer().Serialize(bar), "jQuery('#' + subgrid_table_id).getGridParam('editDialogOptions')", "jQuery('#' + subgrid_table_id).getGridParam('addDialogOptions')", "jQuery('#' + subgrid_table_id).getGridParam('delDialogOptions')", "jQuery('#' + subgrid_table_id).getGridParam('searchDialogOptions')" });
            }
            foreach (Moses.Web.Mvc.Controls.JQGridToolBarButton button in grid.ToolBarSettings.CustomButtons)
            {
                if ((grid.ToolBarSettings.ToolBarPosition == Moses.Web.Mvc.Controls.ToolBarPosition.Bottom) || (grid.ToolBarSettings.ToolBarPosition == Moses.Web.Mvc.Controls.ToolBarPosition.TopAndBottom))
                {
                    builder.AppendFormat(".navButtonAdd({0},{1})", pagerSelectorID, new Moses.Web.Mvc.Controls.JsonCustomButton(button).Process());
                }
                if ((grid.ToolBarSettings.ToolBarPosition == Moses.Web.Mvc.Controls.ToolBarPosition.TopAndBottom) || (grid.ToolBarSettings.ToolBarPosition == Moses.Web.Mvc.Controls.ToolBarPosition.Top))
                {
                    builder.AppendFormat(".navButtonAdd({0},{1})", pagerSelectorID.Replace("_pager", "_toppager"), new Moses.Web.Mvc.Controls.JsonCustomButton(button).Process());
                }
            }
            return builder.ToString();
        }

        public string RenderHtml(Moses.Web.Mvc.Controls.GridControl grid, string detailsPath = "Details", string detailsController = null)
        {
            string format = "<table id='{0}'></table>";
            if (grid.ToolBarSettings.ToolBarPosition != Moses.Web.Mvc.Controls.ToolBarPosition.Hidden)
            {
                format = format + "<div id='{0}_pager'></div>";
            }
            if (string.IsNullOrEmpty(grid.ID))
            {
                throw new Exception("You need to set ID for this grid.");
            }
            format = string.Format(format, grid.ID);
            if ((grid.HierarchySettings.HierarchyMode == Moses.Web.Mvc.Controls.HierarchyMode.Child) || (grid.HierarchySettings.HierarchyMode == Moses.Web.Mvc.Controls.HierarchyMode.ParentAndChild))
            {
                return (format + this.GetChildSubGridJavaScript(grid));
            }
            var s = (format + this.GetStartupJavascript(grid, false));

            var controllerExpression = "";
            if (detailsController != null)
                controllerExpression = "fn-controller=\"" + detailsController + "\" ";

            //hack para transformar uma execução inline em uma execução contextualizada
            s = s.Replace("\r", "").Replace("\n", "").Replace("<table ", "<table class=\"mosesGridInstance\"")
                .Replace("jQuery(document).ready(function() {jQuery('#" + grid.ID + "').jqGrid(", "$context.GridDefinitions['" + grid.ID + "'] = function($grid){ return ")
                .Replace("function jqGrid_aspnet_loadErrorHandler(xht, st, handler) {jQuery(document.body).css('font-size','100%'); jQuery(document.body).html(xht.responseText);}", "")
                .Replace(").bindKeys();", "")
                .Replace("});</script>", ";}</script>");

            return string.Concat(s, "<div id='" + grid.ID + "_grid-rowExpanded' class=\"grid-rowExpanded\" fn-type=\"ManualLoad\" fn-action=\"" + detailsPath + "\" " + controllerExpression + " ></div>");
        }
    }
}

