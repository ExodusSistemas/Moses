using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trirand.Web.Mvc;

namespace Moses.Web.Mvc.Patterns
{
    public abstract class MosesBaseViewModel<T> : MosesBaseViewModel, IMosesPermission
        where T : class, IdContainer<int>, new() 
    {
        T _item = null;
        public T Item
        {
            get
            {
                return _item ?? new T();
            }
            set
            {
                _item = value;
            }
        }

        public override bool IsEdit
        {
            get
            {
                return _item != null ? _item.Id > 0 : false;
            }
        }

        public IQueryable<T> List { get; set; }

        public abstract JQGrid Grid
        {
            get;
        }

    }


    public abstract class MosesBaseViewModel : IMosesPermission
    {
        #region IMosesModel Members

        public abstract bool IsEdit{ get; }

        //define a operação a ser realizada dentro de uma chamada de comando (Save/Delete/MultiEdit)
        
        public MosesOperation Operation { get; set; }

        #endregion

        #region IMosesPermission Members

        
        public MembershipContext MembershipContext { get; set; }

        
        public bool ClienteTrialPermission
        {
            get
            {
                if (MembershipContext == null) return true;
                return MembershipContext.HasPermission("Trial");
            }
            set
            {
            }
        }

        
        public bool ClienteGerenteAnalistaOperadorCaixaPermission
        {
            get
            {
                if (MembershipContext == null) return true;
                return MembershipContext.HasPermission("Gerente", "Analista", "Operador", "Caixa");
            }
            set
            {
            }
        }

        
        public bool ClienteGerenteAnalistaOperadorPermission
        {
            get
            {
                if (MembershipContext == null) return true;
                return MembershipContext.HasPermission("Gerente", "Analista", "Operador");
            }
            set
            {
            }
        }

        
        public bool ClienteGerenteAnalistaPermission
        {
            get
            {
                if (MembershipContext == null) return true;
                return MembershipContext.HasPermission("Gerente", "Analista");
            }
            set
            {
            }
        }

        
        public bool ClienteGerentePermission
        {
            get
            {
                if (MembershipContext == null) return true;
                return MembershipContext.HasPermission("Gerente");
            }
            set
            {
            }
        }

        
        public bool AdministradorPermission
        {
            get
            {
                if (MembershipContext == null) return false;
                return MembershipContext.HasPermission("Trial");
            }
            set
            {
            }
        }

        
        public string RoleName
        {
            get
            {
                return "";
                //return MembershipContext.GetRoleName();
            }
            set
            {
            }
        }

        //página para acesso ao grid
        public int page { get; set; }

        #endregion

        public JQGrid SetDefaultConfigs(ref JQGrid grid, string initialSortColumn = "Id", bool readOnly = false, bool hasDetails = true, int pageSize = 50)
        {
            grid.ID = string.Format("FnGrid_{0}" , DateTime.Now.Ticks.ToString());
            //grid.ClientSideEvents.GridInitialized = "$grid.OnCreateInstance";
            grid.ClientSideEvents.SubGridRowExpanded = hasDetails ? "$grid.GetRowEvent('OnGridRowExpanded')" : null;
            grid.ClientSideEvents.RowSelect = "$grid.GetRowEvent('OnGridRowSelect')";
            grid.MultiSelect = readOnly ? false : true;
            grid.SortSettings.InitialSortColumn = initialSortColumn;
            grid.SortSettings.InitialSortDirection = Trirand.Web.Mvc.SortDirection.Asc;
            grid.PagerSettings.PageSize = pageSize;
            grid.PagerSettings.ScrollBarPaging = false;
            grid.ToolBarSettings.ToolBarPosition = ToolBarPosition.Hidden;
            grid.HierarchySettings.HierarchyMode = hasDetails ? HierarchyMode.Parent : HierarchyMode.None;
            grid.AppearanceSettings.HighlightRowsOnHover = true;
            grid.AppearanceSettings.ShowRowNumbers = true;
            grid.PagerSettings.NoRowsMessage = "Nenhum Registro Encontrado";
            return grid;
        }

        

    }


    
}
