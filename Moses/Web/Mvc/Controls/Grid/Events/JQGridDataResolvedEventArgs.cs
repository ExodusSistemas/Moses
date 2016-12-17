namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Linq;

    public class JQGridDataResolvedEventArgs : EventArgs
    {
        public IQueryable _allData;
        public IQueryable _currentData;
        public Moses.Web.Mvc.Controls.GridControl _gridModel;

        public JQGridDataResolvedEventArgs(Moses.Web.Mvc.Controls.GridControl gridModel, IQueryable currentData, IQueryable allData)
        {
            this._currentData = currentData;
            this._allData = allData;
            this._gridModel = gridModel;
        }

        public IQueryable AllData
        {
            get
            {
                return this._allData;
            }
            set
            {
                this._allData = value;
            }
        }

        public IQueryable CurrentData
        {
            get
            {
                return this._currentData;
            }
            set
            {
                this._currentData = value;
            }
        }

        public Moses.Web.Mvc.Controls.GridControl GridModel
        {
            get
            {
                return this._gridModel;
            }
            set
            {
                this._gridModel = value;
            }
        }
    }
}

