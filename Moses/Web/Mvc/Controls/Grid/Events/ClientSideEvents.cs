namespace Moses.Web.Mvc.Controls
{
    public class ClientSideEvents
    {
        public ClientSideEvents()
        {
            this.RowSelect = "";
            this.RowDoubleClick = "";
            this.RowRightClick = "";
            this.GridInitialized = "";
            this.BeforeAjaxRequest = "";
            this.ServerError = "";
            this.LoadDataError = "";
            this.SubGridRowExpanded = "";
            this.ColumnSort = "";
        }

        public string BeforeAjaxRequest { get; set; }

        public string BeforeDeleteDialogShown { get; set; }

        public string BeforeEditDialogShown { get; set; }

        public string ColumnSort { get; set; }

        public string GridInitialized { get; set; }

        public string LoadDataError { get; set; }

        public string RowDoubleClick { get; set; }

        public string RowRightClick { get; set; }

        public string RowSelect { get; set; }

        public string ServerError { get; set; }

        public string SubGridRowExpanded { get; set; }
    }
}

