using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Moses.Extensions;
using Moses.Web.Mvc.Controls;
using System.Data.Entity;
using Moses.Security;
using System.Linq.Dynamic.Core;

namespace Moses.Web.Mvc.Patterns
{
    [HandleMosesAppException]
    public class BasicController<TEntity, TType, TUnitOfWork, TDataContext, TViewModel> : MosesController
        where TEntity : class, IEntity<TType>, new()
        where TUnitOfWork : class, IUnitOfWork<TEntity, TType, TDataContext>, IGrid<TEntity>, new()
        where TDataContext : DbContext
        where TViewModel : ViewModel<TEntity, TType>, new()
        where TType : struct, IEquatable<TType>

    {
        protected string EntityName { get; set; }
        protected string EntityTitle { get; set; }

        public BasicController() 
        {
            EntityName = Manager.GetEntityName();
            EntityTitle = Manager.GetEntityName();
        }

        private TUnitOfWork _manager = null;
        public virtual TUnitOfWork Manager
        {
            get
            {
                if (_manager == null)
                {
                    _manager = new TUnitOfWork();
                }
                return _manager;
            }
        }

        //
        // GET: 
        [Feature(FeatureOptions.Read)]
        public virtual ActionResult List()
        {
            TViewModel info = new TViewModel();
            info.Grid = Manager.GetGrid();
            info.Grid.ID = "GridControl1";
            info.Grid.DataUrl = Url.Action("DataRequested");
            info.Grid.SortSettings.InitialSortDirection = SortDirection.Desc;

            return View("List" + EntityName + "s", info);
        }

        [Feature(FeatureOptions.Read)]
        public virtual ActionResult DataRequested(TViewModel info)
        {
            info.List = Manager.GetAll();
            info.Grid = Manager.GetGrid();
            return info.Grid.DataBind(info.List);
        }

        //
        // GET: /T/Details/5
        [Feature(FeatureOptions.Modify)]
        public virtual ActionResult Add(TViewModel info = null)
        {
            info = info ?? new TViewModel();
            info.Item = Manager.InitializeEntity();
            return View("Form" + EntityName, info);
        }

        [Feature(FeatureOptions.Modify)]
        public virtual ActionResult Duplicate(TType? sourceId)
        {
            if (sourceId == null)
                return Add(null);
            else
            {
                var info = new TViewModel() { Item = Manager.Copy(sourceId.Value) };
                Manager.InitializeEntity(info.Item);
                return View("Form" + EntityName, info);
            }
        }

        [Feature(FeatureOptions.Modify)]
        public virtual ActionResult Edit(TViewModel info)
        {
            info = info ?? new TViewModel();
            var id = info.Item.Id;
            if (id.Equals(default(TType)))
                return RedirectToAction("List", new { ErrorMessage = EntityTitle + " inválido" });

            info.Item = Manager.Get(id);
            return View("Form" + EntityName, info);
        }

        [Feature(FeatureOptions.Modify)]
        public virtual ActionResult Delete(string rowIds)
        {
            try
            {
                List<TType> result = Manager.DeleteAll(rowIds);
                Manager.SubmitChanges();

                return Deleted(EntityTitle + "(s) excluído(s) com sucesso", result);
            }
            catch (MosesApplicationException mex)
            {
                return Fail(mex.Message);
            }
            catch (Exception ex)
            {
                return Fail("Falha ao excluir o(s) " + EntityTitle + "(s):" + ex.Message);
            }
        }

        [HttpPost]
        [Feature(FeatureOptions.Modify)]
        public virtual ActionResult Save(TViewModel info)
        {
            try
            {
                var isEdit = info.IsEdit; //armazena antes de executar a operação, pois o valor pode mudar
                info.Item = Manager.Save(info.Item);
                Manager.SubmitChanges();

                return Saved(EntityName + (isEdit ? "alterado" : "cadastrado") + " com sucesso", info);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dex)
            {
                var errorMsg = dex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                return Fail(string.Join("; ", errorMsg));
            }
            catch (MosesApplicationException mex)
            {
                return Fail(mex.StackTrace +  mex.Message);
            }
        }

        [Feature(FeatureOptions.Read)]
        public virtual ActionResult Details(TType id)
        {
            return Manager.Details(id);
        }

        [Feature(FeatureOptions.Read)]
        public virtual string AutoComplete(AutoCompleteControl<TEntity> q)
        {
            if (!q.All && string.IsNullOrEmpty(q.Term) )
                return "";

            if (Manager is IAutoComplete)
            {
                q.DataSource = this.Manager.GetAll();
                if ( !q.All){
                    var linqExpression = $"{q.DataField}.Contains(@0)";
                    if ( q.AutoCompleteMode == Moses.Web.Mvc.Controls.AutoCompleteMode.BeginsWith)
                    {
                        linqExpression = $"{q.DataField}.StartsWith(@0)";
                    }
                    var autoCompleteManager = Manager as IAutoComplete;
                    var filteredList = autoCompleteManager.GetSerializableList(q.DataSource.Where(linqExpression, q.Term).Take(q.Max));
                    return filteredList.ToJSon();
                }
                else{
                    return q.DataSource.ToJSon();
                }
            }
            else
            {
                throw new MosesApplicationException("IAutoCompleteUnitOfWork não implementado");
            }

        }

        
    }

}

