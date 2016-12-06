using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using System.Collections;
using System.Web.Mvc;
using Trirand.Web.Mvc;
using System.Data;

namespace Moses.Web.Extensions
{
    public static class JqGridDataHelper
    {
       
        public static string JsonForJqgrid(this JQGrid grid, DataTable dt, int page = 1)
        {
            int pageSize = grid.PagerSettings.PageSize;
            int totalRecords = dt.Rows.Count;
            //page = grid.PagerSettings.CurrentPage;

            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var limitDown = (page - 1) * pageSize + 1;
            var limitUp = (page) * pageSize;

            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{");
            jsonBuilder.Append("\"total\":" + totalPages + ",\"page\":" + page + ",\"records\":" + (totalRecords) + ",\"rows\"");
            jsonBuilder.Append(":[");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if ((i + 1) < limitDown || (i + 1) > limitUp) continue;
                jsonBuilder.Append("{\"i\":" + (i) + ",\"cell\":[");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("]},");
            }

            //resolve o BUG do grid aparecer deixar a tela toda branca
            if (dt.Rows.Count == 0)
            {
                jsonBuilder.Append("0");
            }

            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();

        }
       

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Jq",
            Justification = "JqGrid is the correct name of the JavaScript component this type is designed to support.")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Jq",
            Justification = "JqGrid is the correct name of the grid component we use.")]
        public static JqGridData ToJqGridData<T>(this PagedList<T> list, object userData)
        {
            return new JqGridData()
            {
                Page = list.PageIndex,
                Records = list.TotalCount,
                Rows = from record in list
                       select record,
                Total = list.PageIndex,
                UserData = userData
            };
        }

        public static JqGridData ToJqGridData<T>(this List<T> list, int page, int idx, object userData)
        {
            return new JqGridData()
            {
                Page = page,
                Records = list.Count,
                Rows = from record in list
                       select record,
                Total = idx,
                UserData = userData
            };
        }

        /// <summary>
        /// Adds a Where to a Queryable list of entity instances.  In other words, filter the list
        /// based on the search parameters passed.
        /// </summary>
        /// <typeparam name="T">Entity type contained within the list</typeparam>
        /// <param name="baseList">Unfiltered list</param>
        /// <param name="searchQuery">Whatever the user typed into the search box</param>
        /// <param name="searchColumns">List of entity properties which should be included in the
        /// search.  If any property in an entity instance begins with the search query, it will
        /// be included in the result.</param>
        /// <returns>Filtered list.  Note that the query will not actually be executed until the
        /// IQueryable is enumerated.</returns>
        //private static IQueryable<T> ListAddSearchQuery<T>(
        //    IQueryable<T> baseList,
        //    string searchQuery,
        //    IEnumerable<string> searchColumns)
        //{
        //    if ((String.IsNullOrEmpty(searchQuery)) | (searchColumns == null)) return baseList;
        //    const string strpredicateFormat = "{0}.ToString().StartsWith(@0)";
        //    var searchExpression = new System.Text.StringBuilder();
        //    string orPart = String.Empty;
        //    foreach (string column in searchColumns)
        //    {
        //        searchExpression.Append(orPart);
        //        searchExpression.AppendFormat(strpredicateFormat, column, searchQuery);
        //        orPart = " OR ";
        //    }
        //    var filteredList = baseList.Where(searchExpression.ToString(), searchQuery);
        //    return filteredList;
        //}

        /// <summary>
        /// Converts a queryable expression into a format suitable for the JqGrid component, when
        /// serialized to JSON. Use this method when returning data that the JqGrid component will
        /// fetch via AJAX. Take the result of this method, and then serialize to JSON. This method
        /// will also apply paging to the data.
        /// </summary>
        /// <typeparam name="T">The type of the element in baseList. Note that this type should be
        /// an anonymous type or a simple, named type with no possibility of a cycle in the object
        /// graph. The default JSON serializer will throw an exception if the object graph it is
        /// serializing contains cycles.</typeparam>
        /// <param name="baseList">The list of records to display in the grid.</param>
        /// <param name="currentPage">A 1-based index indicating which page the grid is about to display.</param>
        /// <param name="rowsPerPage">The maximum number of rows which the grid can display at the moment.</param>
        /// <param name="orderBy">A string expression containing a column name and an optional ASC or
        /// DESC. You can, separate multiple column names as with SQL.</param>
        /// <param name="searchQuery">The value which the user typed into the search box, if any. Can be
        /// null/empty.</param>
        /// <param name="searchColumns">An array of strings containing the names of properties in the
        /// element type of baseList which should be considered when searching the data for searchQuery.</param>
        /// <param name="userData">Arbitrary data to be returned to the grid along with the row data. Leave
        /// null if not using. Must be serializable to JSON!</param>
        /// <returns>A structure containing all of the fields required by the JqGrid. You can then call
        /// a JSON serializer, passing this result.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Jq",
            Justification = "JqGrid is the correct name of the JavaScript component this type is designed to support.")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Jq",
            Justification = "JqGrid is the correct name of the grid component we use.")]
        public static JqGridData ToJqGridData<T>(this IQueryable<T> baseList,
            int currentPage,
            int rowsPerPage,
            string orderBy,
            string searchQuery,
            IEnumerable<string> searchColumns,
            object userData)
        {

            throw new NotImplementedException();
            //var filteredList = ListAddSearchQuery(baseList, searchQuery, searchColumns);
            //var pagedModel = null;// new PagedList<T>(filteredList.OrderBy(orderBy), currentPage, rowsPerPage);
            //return pagedModel.ToJqGridData(userData);
        }
    }


    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Jq",
        Justification = "JqGrid is the correct name of the JavaScript component this type is designed to support.")]
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Jq",
        Justification = "JqGrid is the correct name of the JavaScript component this type is designed to support.")]
    public class JqGridData
    {
        /// <summary>
        /// The number of pages which should be displayed in the paging controls at the bottom of the grid.
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// The current page number which should be highlighted in the paging controls at the bottom of the grid.
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// The total number of records in the entire data set, not just the portion returned in Rows.
        /// </summary>
        public int Records { get; set; }
        /// <summary>
        /// The data that will actually be displayed in the grid.
        /// </summary>
        public IEnumerable Rows { get; set; }
        /// <summary>
        /// Arbitrary data to be returned to the grid along with the row data. Leave null if not using. Must be serializable to JSON!
        /// </summary>
        public object UserData { get; set; }

        

        
    }




}
