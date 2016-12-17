using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Moses.Web.Mvc.Controls;

namespace Moses.Web.Mvc.Controls
{

    

    /// <summary>
    /// Contém implementações básicas do Moses
    /// </summary>
    public static class ColumnBuilder
    {
        public static void AddColumnKey<TModel, TProperty>(this GridControl<TModel> grid, Expression<Func<TModel, TProperty>> expression, string title, bool searchable = true, bool visible = true)
            where TModel : class, new()
            where TProperty : struct
        {
            ColumnBuilder.AddColumnBase(grid, ColumnBuilder.Column, expression, title, searchable, visible, 80);
        }

        public static void AddColumnDate<TModel>(this GridControl<TModel> grid, Expression<Func<TModel, DateTime>> expression, string title, bool searchable = false, bool visible = true)
            where TModel : class, new()
        {
            ColumnBuilder.AddColumnBase(grid, ColumnBuilder.DateColumn, expression, title, searchable, visible, 80);
        }

        public static void AddColumnCurrency<TModel>(this GridControl<TModel> grid, Expression<Func<TModel, decimal>> expression, string title, bool searchable = false, int width = 75)
            where TModel : class, new()
        {
            ColumnBuilder.AddColumnBase(grid, ColumnBuilder.CurrencyColumn, expression, title, searchable, true, width);
        }

        public static void AddColumnCurrencyMonthly<TModel>(this GridControl<TModel> grid, Expression<Func<TModel, decimal>> expression, string title, bool searchable = false, int width = 75)
            where TModel : class, new()
        {
            ColumnBuilder.AddColumnBase(grid, ColumnBuilder.CurrencyMonthlyColumn, expression, title, searchable, true, width);
        }

        public static void AddColumnEmail<TModel>(this GridControl<TModel> grid, Expression<Func<TModel, string>> expression, string title, bool searchable = true, bool visible = true, int width = 165)
            where TModel : class, new()
        {
            ColumnBuilder.AddColumnBase(grid, ColumnBuilder.Column, expression, title, searchable, visible, width);
        }

        public static void AddColumn<TModel, TProperty>(this GridControl<TModel> grid, Expression<Func<TModel, TProperty>> expression, string title = null, bool searchable = true, bool visible = true, int width = 165)
            where TModel : class, new()
        {
            ColumnBuilder.AddColumnBase(grid, ColumnBuilder.Column, expression, title, searchable, visible, width);
        }

        private static void AddColumnBase<TModel, TProperty>(this GridControl<TModel> grid, Func<Type, string, string, bool, bool, int, GridControlColumn> func, Expression<Func<TModel, TProperty>> expression, string title = null, bool searchable = true, bool visible = true, int width = 165)
            where TModel : class, new()
        {
            MemberExpression memberExpression = (MemberExpression)expression.Body;
            var propertyName = memberExpression.Member is PropertyInfo ? memberExpression.Member.Name : null;
            if (title == null)
                title = propertyName;

            grid.Columns.Add(func(memberExpression.Type, propertyName, title ?? propertyName, searchable, visible, width));

        }

        public static GridControlColumn Column(Type dataType, string dataField, string headerText = null, bool searchable = true, bool visible = true, int width = 165)
        {
            return new GridControlColumn
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

        internal static GridControlColumn DateColumn(Type dataType, string dataField, string headerText = null, bool searchable = true, bool visible = true, int width = 80)
        {

            return new GridControlColumn//6
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
                Formatter = DateFormat()
            };

        }

        internal static GridControlColumn CurrencyColumn(Type dataType, string dataField, string headerText = null, bool searchable = false, bool visible = true, int width = 75)
        {
            return new GridControlColumn//8
            {
                DataField = dataField,
                HeaderText = headerText ?? dataField,
                Width = width,
                Visible = visible,
                DataType = typeof(decimal),
                TextAlign = Moses.Web.Mvc.Controls.TextAlign.Right,
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

        internal static GridControlColumn EmailColumn(Type dataType, string dataField, string headerText = null, bool searchable = true, bool visible = true, int width = 165)
        {
            return new GridControlColumn
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

        internal static GridControlColumn CurrencyMonthlyColumn(Type dataType, string dataField, string headerText, bool searchable = false, bool visible = true, int width = 70)
        {
            return new GridControlColumn() //0
            {
                HeaderText = headerText,
                DataField = dataField,
                DataType = typeof(System.Decimal),
                Visible = visible,
                PrimaryKey = false,
                Width = width,
                TextAlign = Moses.Web.Mvc.Controls.TextAlign.Right,
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