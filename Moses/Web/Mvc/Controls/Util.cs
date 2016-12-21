namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Web.Mvc;

    internal static class Util
    {
        // Methods
        internal static string ConstructLinqFilterExpressionSingle<T>(AutoCompleteControl<T> autoComplete, SearchArguments args)
        {
            Guard.IsNotNull(autoComplete.DataField, "DataField", "must be set in order to perform search operations. If you get this error from search/export method, make sure you setup(initialize) the grid again prior to filtering/exporting.");
            string filterExpressionCompare = "{0} {1} @0";
            return GetLinqExpression(filterExpressionCompare, args, false, typeof(string));
        }

        internal static string ConstructLinqFilterExpression<T>(AutoCompleteControl<T> autoComplete, SearchArguments args)
        {
            Guard.IsNotNull(autoComplete.DataField, "DataField", "must be set in order to perform search operations. If you get this error from search/export method, make sure you setup(initialize) the grid again prior to filtering/exporting.");
            string filterExpressionCompare = "{0} {1} \"{2}\"";
            return GetLinqExpression(filterExpressionCompare, args, false, typeof(string));
        }

        private static string ConstructLinqFilterExpression(GridControl grid, SearchArguments args)
        {
            GridControlColumn column = grid.Columns.Find(c => c.DataField == args.SearchColumn);
            if (column.DataType == null)
            {
                throw new DataTypeNotSetException("JQGridColumn.DataType must be set in order to perform search operations.");
            }
            string filterExpressionCompare = (column.DataType == typeof(string)) ? "{0} {1} \"{2}\"" : "{0} {1} {2}";
            if (column.DataType == typeof(DateTime))
            {
                DateTime time = DateTime.Parse(args.SearchString);
                string str2 = $"({time.Year},{time.Month},{time.Day})";
                filterExpressionCompare = "{0} {1} DateTime" + str2;
            }
            return ($"{args.SearchColumn} != null AND " + GetLinqExpression(filterExpressionCompare, args, column.SearchCaseSensitive, column.DataType));
        }

        internal static JsonResult ConvertToJson(JsonResponse response, GridControl grid, DataTable dt)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = 0;
            if (response.records == 0)
            {
                result.Data = new object[0];
                return result;
            }
            result.Data = PrepareJsonResponse(response, grid, dt);
            return result;
        }

        internal static string ConvertToJsonString(JsonResponse response, GridControl grid, DataTable dt) =>
            Configuration.Json.Serialize(PrepareJsonResponse(response, grid, dt));

        public static Hashtable GetFooterInfo(GridControl grid)
        {
            Hashtable hashtable = new Hashtable();
            if (grid.AppearanceSettings.ShowFooter)
            {
                foreach (GridControlColumn column in grid.Columns)
                {
                    hashtable[column.DataField] = column.FooterValue;
                }
            }
            return hashtable;
        }

        private static string GetLinqExpression(string filterExpressionCompare, SearchArguments args, bool caseSensitive, Type dataType)
        {
            string str = caseSensitive ? args.SearchString : args.SearchString.ToLower();
            string searchColumn = args.SearchColumn;
            if (((dataType != null) && (dataType == typeof(string))) && !caseSensitive)
            {
                searchColumn = $"{args.SearchColumn}.ToLower()";
            }
            switch (args.SearchOperation)
            {
                case SearchOperation.IsEqualTo:
                    return string.Format(filterExpressionCompare, searchColumn, "=", str);

                case SearchOperation.IsNotEqualTo:
                    return string.Format(filterExpressionCompare, searchColumn, "<>", str);

                case SearchOperation.IsLessThan:
                    return string.Format(filterExpressionCompare, searchColumn, "<", str);

                case SearchOperation.IsLessOrEqualTo:
                    return string.Format(filterExpressionCompare, searchColumn, "<=", str);

                case SearchOperation.IsGreaterThan:
                    return string.Format(filterExpressionCompare, searchColumn, ">", str);

                case SearchOperation.IsGreaterOrEqualTo:
                    return string.Format(filterExpressionCompare, searchColumn, ">=", str);

                case SearchOperation.BeginsWith:
                    return $"{searchColumn}.StartsWith(\"{str}\")";

                case SearchOperation.DoesNotBeginWith:
                    return $"!{searchColumn}.StartsWith(\"{str}\")";

                case SearchOperation.EndsWith:
                    return $"{searchColumn}.EndsWith(\"{str}\")";

                case SearchOperation.DoesNotEndWith:
                    return $"!{searchColumn}.EndsWith(\"{str}\")";

                case SearchOperation.Contains:
                    return $"{searchColumn}.Contains(\"{str}\")";

                case SearchOperation.DoesNotContain:
                    return $"!{searchColumn}.Contains(\"{str}\")";
            }
            throw new Exception("Invalid search operation.");
        }

        public static string GetPrimaryKeyField(GridControl grid)
        {
            int primaryKeyIndex = GetPrimaryKeyIndex(grid);
            return grid.Columns[primaryKeyIndex].DataField;
        }

        public static int GetPrimaryKeyIndex(GridControl grid)
        {
            foreach (GridControlColumn column in grid.Columns)
            {
                if (column.PrimaryKey)
                {
                    return grid.Columns.IndexOf(column);
                }
            }
            return 0;
        }

        private static SearchOperation GetSearchOperationFromString(string searchOperation)
        {
            switch (searchOperation)
            {
                case "eq":
                    return SearchOperation.IsEqualTo;

                case "ne":
                    return SearchOperation.IsNotEqualTo;

                case "lt":
                    return SearchOperation.IsLessThan;

                case "le":
                    return SearchOperation.IsLessOrEqualTo;

                case "gt":
                    return SearchOperation.IsGreaterThan;

                case "ge":
                    return SearchOperation.IsGreaterOrEqualTo;

                case "in":
                    return SearchOperation.IsIn;

                case "ni":
                    return SearchOperation.IsNotIn;

                case "bw":
                    return SearchOperation.BeginsWith;

                case "bn":
                    return SearchOperation.DoesNotBeginWith;

                case "ew":
                    return SearchOperation.EndsWith;

                case "en":
                    return SearchOperation.DoesNotEndWith;

                case "cn":
                    return SearchOperation.Contains;

                case "nc":
                    return SearchOperation.DoesNotContain;
            }
            throw new Exception("Search operation not known: " + searchOperation);
        }

        public static string GetWhereClause(GridControl grid, NameValueCollection queryString)
        {
            string str = " && ";
            string str2 = "";
            new Hashtable();
            foreach (GridControlColumn column in grid.Columns)
            {
                string str3 = queryString[column.DataField];
                if (!string.IsNullOrEmpty(str3))
                {
                    SearchArguments args = new SearchArguments
                    {
                        SearchColumn = column.DataField,
                        SearchString = str3,
                        SearchOperation = column.SearchToolBarOperation
                    };
                    string str4 = (str2.Length > 0) ? str : "";
                    string str5 = ConstructLinqFilterExpression(grid, args);
                    str2 = str2 + str4 + str5;
                }
            }
            return str2;
        }

        public static string GetWhereClause(GridControl grid, string filters)
        {
            JsonMultipleSearch search = Configuration.Json.Deserialize<JsonMultipleSearch>(filters);
            string str = "";
            foreach (MultipleSearchRule rule in search.rules)
            {
                SearchArguments args = new SearchArguments
                {
                    SearchColumn = rule.field,
                    SearchString = rule.data,
                    SearchOperation = GetSearchOperationFromString(rule.op)
                };
                string str2 = (str.Length > 0) ? (" " + search.groupOp + " ") : "";
                str = str + str2 + ConstructLinqFilterExpression(grid, args);
            }
            return str;
        }

        public static string GetWhereClause(GridControl grid, string searchField, string searchString, string searchOper)
        {
            string str = " && ";
            string str2 = "";
            new Hashtable();
            SearchArguments args = new SearchArguments
            {
                SearchColumn = searchField,
                SearchString = searchString,
                SearchOperation = GetSearchOperationFromString(searchOper)
            };
            string str3 = (str2.Length > 0) ? str : "";
            string str4 = ConstructLinqFilterExpression(grid, args);
            return (str2 + str3 + str4);
        }

        internal static JsonResponse PrepareJsonResponse(JsonResponse response, GridControl grid, DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string[] strArray = new string[grid.Columns.Count];
                for (int j = 0; j < grid.Columns.Count; j++)
                {
                    GridControlColumn column = grid.Columns[j];
                    string str = "";
                    if (!string.IsNullOrEmpty(column.DataField))
                    {
                        int ordinal = dt.Columns[column.DataField].Ordinal;
                        str = string.IsNullOrEmpty(column.DataFormatString) ? dt.Rows[i].ItemArray[ordinal].ToString() : column.FormatDataValue(dt.Rows[i].ItemArray[ordinal], column.HtmlEncode);
                    }
                    strArray[j] = str;
                }
                string str2 = strArray[GetPrimaryKeyIndex(grid)];
                JsonRow row = new JsonRow
                {
                    id = str2,
                    cell = strArray
                };
                response.rows[i] = row;
            }
            return response;
        }

        // Nested Types
        internal class SearchArguments
        {
            // Properties
            public string SearchColumn { get; set; }

            public SearchOperation SearchOperation { get; set; }

            public string SearchString { get; set; }
        }
    }




}

