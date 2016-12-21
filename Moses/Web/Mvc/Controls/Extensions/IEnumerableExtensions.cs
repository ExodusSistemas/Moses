namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    internal static class IEnumerableExtensions
    {
        private static DataTable ObtainDataTableFromIEnumerable(IEnumerable ien, Moses.Web.Mvc.Controls.GridControl grid)
        {
            DataTable table = new DataTable();
            foreach (object obj2 in ien)
            {
                if (obj2 is DbDataRecord)
                {
                    DbDataRecord record = obj2 as DbDataRecord;
                    if (table.Columns.Count == 0)
                    {
                        foreach (Moses.Web.Mvc.Controls.GridControlColumn column in grid.Columns)
                        {
                            table.Columns.Add(column.DataField);
                        }
                    }
                    DataRow row = table.NewRow();
                    foreach (Moses.Web.Mvc.Controls.GridControlColumn column2 in grid.Columns)
                    {
                        row[column2.DataField] = record[column2.DataField];
                    }
                    table.Rows.Add(row);
                }
                else if (obj2 is DataRow)
                {
                    DataRow row2 = obj2 as DataRow;
                    if (table.Columns.Count == 0)
                    {
                        foreach (Moses.Web.Mvc.Controls.GridControlColumn column3 in grid.Columns)
                        {
                            table.Columns.Add(column3.DataField);
                        }
                    }
                    DataRow row3 = table.NewRow();
                    foreach (Moses.Web.Mvc.Controls.GridControlColumn column4 in grid.Columns)
                    {
                        row3[column4.DataField] = row2[column4.DataField];
                    }
                    table.Rows.Add(row3);
                }
                else
                {
                    PropertyInfo[] properties = obj2.GetType().GetProperties();
                    if (table.Columns.Count == 0)
                    {
                        foreach (PropertyInfo info in properties)
                        {
                            Type propertyType = info.PropertyType;
                            if (propertyType.IsGenericType && (propertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                            {
                                propertyType = Nullable.GetUnderlyingType(propertyType);
                            }
                            table.Columns.Add(info.Name, propertyType);
                        }
                    }
                    DataRow row4 = table.NewRow();
                    foreach (PropertyInfo info2 in properties)
                    {
                        object obj3 = info2.GetValue(obj2, null);
                        if (obj3 != null)
                        {
                            row4[info2.Name] = obj3;
                        }
                        else
                        {
                            row4[info2.Name] = DBNull.Value;
                        }
                    }
                    table.Rows.Add(row4);
                }
            }
            return table;
        }

        public static DataTable ToDataTable<T>(this IEnumerable en, Moses.Web.Mvc.Controls.AutoCompleteControl<T> autoComplete)
        {
            Moses.Web.Mvc.Controls.GridControl grid = new Moses.Web.Mvc.Controls.GridControl();
            Moses.Web.Mvc.Controls.GridControlColumn item = new Moses.Web.Mvc.Controls.GridControlColumn {
                DataField = autoComplete.DataField
            };
            grid.Columns.Add(item);
            return en.ToDataTable(grid);
        }

        public static DataTable ToDataTable(this IEnumerable en, Moses.Web.Mvc.Controls.GridControl grid)
        {
            DataTable table = new DataTable();
            DataView view = en as DataView;
            if (view != null)
            {
                return view.ToTable();
            }
            if (en != null)
            {
                table = ObtainDataTableFromIEnumerable(en, grid);
            }
            return table;
        }

        public static List<string> ToListOfString<T>(this IEnumerable en, Moses.Web.Mvc.Controls.AutoCompleteControl<T> autoComplete)
        {
            DataTable table = en.ToDataTable(autoComplete);
            List<string> list = new List<string>();
            IEnumerator enumerator = table.Rows.GetEnumerator();
            
            Predicate<string> match = null;
            DataRow row;
            while (enumerator.MoveNext())
            {
                row = (DataRow) enumerator.Current;
                if (match == null)
                {
                    match = s => s == row[autoComplete.DataField].ToString();
                }
                if (string.IsNullOrEmpty(list.Find(match)))
                {
                    list.Add(row[autoComplete.DataField].ToString());
                }
            }

            enumerator = null;


            return list;
        }
    }
}

