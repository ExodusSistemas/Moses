namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;

    internal class JsonResponse
    {
        public JsonResponse(int currentPage, int totalPagesCount, int totalRowCount, int pageSize, int actualRows, Hashtable userData)
        {
            this.page = currentPage;
            this.total = totalPagesCount;
            this.records = totalRowCount;
            this.rows = new Moses.Web.Mvc.Controls.JsonRow[actualRows];
            this.userdata = userData;
        }

        public int page { get; set; }

        public int records { get; set; }

        public Moses.Web.Mvc.Controls.JsonRow[] rows { get; set; }

        public int total { get; set; }

        public Hashtable userdata { get; set; }
    }
}

