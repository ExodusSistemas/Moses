namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Runtime.CompilerServices;

    public class PagerSettings
    {
        public PagerSettings()
        {
            this.PageSize = 10;
            this.CurrentPage = 1;
            this.PageSizeOptions = "[10,20,30,50]";
            this.NoRowsMessage = "";
            this.ScrollBarPaging = false;
            this.PagingMessage = "";
        }

        public int CurrentPage { get; set; }

        public string NoRowsMessage { get; set; }

        public int PageSize { get; set; }

        public string PageSizeOptions { get; set; }

        public string PagingMessage { get; set; }

        public bool ScrollBarPaging { get; set; }
    }
}

