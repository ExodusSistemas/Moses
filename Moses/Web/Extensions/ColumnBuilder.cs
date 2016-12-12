using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Trirand.Web.Mvc;

namespace Moses.Extensions
{

    /// <summary>
    /// Extensão do JQGrid 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JQGrid<T> : JQGrid
        where T : class, new()
    {
        public JQGrid()
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
            this.SortSettings.InitialSortDirection = Trirand.Web.Mvc.SortDirection.Asc;
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

    /// <summary>
    /// Contém implementações básicas do Moses
    /// </summary>
    public static class ColumnBuilder
    {
        public static void AddColumnKey<TModel, TProperty>(this JQGrid<TModel> grid, Expression<Func<TModel, TProperty>> expression, string title, bool searchable = true, bool visible = true)
            where TModel : class, new()
            where TProperty : struct
        {
            ColumnBuilder.AddColumnBase(grid, ColumnBuilder.Column, expression, title, searchable, visible, 80);
        }

        public static void AddColumnDate<TModel>(this JQGrid<TModel> grid, Expression<Func<TModel, DateTime>> expression, string title, bool searchable = false, bool visible = true)
            where TModel : class, new()
        {
            ColumnBuilder.AddColumnBase(grid, ColumnBuilder.DateColumn, expression, title, searchable, visible, 80);
        }

        public static void AddColumnCurrency<TModel>(this JQGrid<TModel> grid, Expression<Func<TModel, decimal>> expression, string title, bool searchable = false, int width = 75)
            where TModel : class, new()
        {
            ColumnBuilder.AddColumnBase(grid, ColumnBuilder.CurrencyColumn, expression, title, searchable, true, width);
        }

        public static void AddColumnCurrencyMonthly<TModel>(this JQGrid<TModel> grid, Expression<Func<TModel, decimal>> expression, string title, bool searchable = false, int width = 75)
            where TModel : class, new()
        {
            ColumnBuilder.AddColumnBase(grid, ColumnBuilder.CurrencyMonthlyColumn, expression, title, searchable, true, width);
        }

        public static void AddColumnEmail<TModel>(this JQGrid<TModel> grid, Expression<Func<TModel, string>> expression, string title, bool searchable = true, bool visible = true, int width = 165)
            where TModel : class, new()
        {
            ColumnBuilder.AddColumnBase(grid, ColumnBuilder.Column, expression, title, searchable, visible, width);
        }

        public static void AddColumn<TModel, TProperty>(this JQGrid<TModel> grid, Expression<Func<TModel, TProperty>> expression, string title = null, bool searchable = true, bool visible = true, int width = 165)
            where TModel : class, new()
        {
            ColumnBuilder.AddColumnBase(grid, ColumnBuilder.Column, expression, title, searchable, visible, width);
        }

        private static void AddColumnBase<TModel, TProperty>(this JQGrid<TModel> grid, Func<Type, string, string, bool, bool, int, JQGridColumn> func, Expression<Func<TModel, TProperty>> expression, string title = null, bool searchable = true, bool visible = true, int width = 165)
            where TModel : class, new()
        {
            MemberExpression memberExpression = (MemberExpression)expression.Body;
            var propertyName = memberExpression.Member is PropertyInfo ? memberExpression.Member.Name : null;
            if (title == null)
                title = propertyName;

            grid.Columns.Add(func(memberExpression.Type, propertyName, title ?? propertyName, searchable, visible, width));

        }

        public static JQGridColumn Column(Type dataType, string dataField, string headerText = null, bool searchable = true, bool visible = true, int width = 165)
        {
            return new JQGridColumn
            {
                HeaderText = headerText ?? dataField,
                Width = width,
                Searchable = searchable,
                DataField = dataField,
                DataType = dataType,
                SearchToolBarOperation = dataType == typeof(string) ? SearchOperation.Contains : SearchOperation.IsEqualTo,
                Visible = visible,
                SearchCaseSensitive = false,
                Formatter = FormatOnClient()
            };
        }

        #region Implementações de Column

        internal static JQGridColumn DateColumn(Type dataType, string dataField, string headerText = null, bool searchable = true, bool visible = true, int width = 80)
        {

            return new JQGridColumn//6
            {
                HeaderText = headerText ?? dataField,
                DataField = dataField,
                Width = width,
                Visible = visible,
                // Para Busca
                Searchable = searchable,
                DataType = typeof(DateTime?),
                SearchToolBarOperation = SearchOperation.IsEqualTo,
                SearchType = SearchType.DatePicker,
                DataFormatString = "{0:d}",
                EditType = EditType.DatePicker,
                EditorControlID = "DatePicker",
                Formatter = DateFormat()
            };

        }

        internal static JQGridColumn CurrencyColumn(Type dataType, string dataField, string headerText = null, bool searchable = false, bool visible = true, int width = 75)
        {
            return new JQGridColumn//8
            {
                DataField = dataField,
                HeaderText = headerText ?? dataField,
                Width = width,
                Visible = visible,
                DataType = typeof(decimal),
                TextAlign = Trirand.Web.Mvc.TextAlign.Right,
                SearchType = SearchType.TextBox,
                Searchable = searchable,
                SearchToolBarOperation = SearchOperation.IsEqualTo,
                //DataFormatString = "{0:C}",
                Formatter = new CustomFormatter
                {
                    FormatFunction = "$grid.Formatters.CurrencyFormat",
                    UnFormatFunction = "$grid.UnformatColumn"
                }
            };
        }

        internal static JQGridColumn EmailColumn(Type dataType, string dataField, string headerText = null, bool searchable = true, bool visible = true, int width = 165)
        {
            return new JQGridColumn
            {
                HeaderText = headerText ?? dataField,
                Width = width,
                Searchable = searchable,
                DataField = dataField,
                DataType = typeof(string),
                SearchToolBarOperation = SearchOperation.Contains,
                Visible = visible,
                Formatter = new CustomFormatter
                {
                    FormatFunction = "$grid.Formatters.EmailFormat",
                    UnFormatFunction = "$grid.UnformatColumn"
                }
            };

        }

        internal static JQGridColumn CurrencyMonthlyColumn(Type dataType, string dataField, string headerText, bool searchable = false, bool visible = true, int width = 70)
        {
            return new JQGridColumn() //0
            {
                HeaderText = headerText,
                DataField = dataField,
                DataType = typeof(System.Decimal),
                Visible = visible,
                PrimaryKey = false,
                Editable = false,
                Width = width,
                TextAlign = Trirand.Web.Mvc.TextAlign.Right,
                Searchable = searchable,
                SearchToolBarOperation = SearchOperation.IsEqualTo,
                Formatter = new CustomFormatter
                {
                    FormatFunction = "$grid.Formatters.CurrencyMonthlyFormat",
                    UnFormatFunction = "$grid.UnformatColumn"
                }
            };
        }

        #endregion

        internal static JQGridColumnFormatter DateFormat()
        {
            return new CustomFormatter()
            {
                FormatFunction = "$grid.Formatters.FormatDate",
                UnFormatFunction = "$grid.UnformatColumn"
            };

        }

        public static JQGridColumnFormatter FormatOnClient()
        {
            return new CustomFormatter()
            {
                FormatFunction = "$grid.GetGenericRowSchemeFormatter()",
                UnFormatFunction = "function(){}"
            };
        }
    }
}