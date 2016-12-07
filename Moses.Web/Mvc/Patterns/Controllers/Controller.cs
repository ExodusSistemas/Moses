using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Moses.Extensions;
using Trirand.Web.Mvc;
using System.Data.Entity;
using Moses.Security;

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
        // GET: /T/
        [Feature(FeatureOptions.Read)]
        public virtual ActionResult List()
        {
            TViewModel info = new TViewModel();
            info.Grid = Manager.GetGrid();
            info.Grid.ID = "GvResultadoJq";
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
                return Fail("Falha ao excluir o(s) " + EntityTitle + "(s)");
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
        public virtual string AutoComplete(TType? id)
        {
            var list = this.Manager.GetAll();

            if (id != null)
                list = list.Where(q => q.Id.Equals(id));

            if (Manager is IAutoComplete)
            {
                var autoCompleteManager = Manager as IAutoComplete;
                var safeList = autoCompleteManager.GetSerializableList(list);
                return safeList.ToJSon() as string;
            }
            else
            {
                throw new MosesApplicationException("IAutoCompleteUnitOfWork não implementado");
            }

        }
    }

}

