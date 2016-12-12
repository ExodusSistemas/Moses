using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moses.Web.Mvc;
using System.Text;
using System.Security.Principal;
using System.Web.UI;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.Caching;
using Moses.Web;
using Moses.Membership;

namespace Moses.Web.Mvc.Patterns
{
   
        /// <summary>
        /// Representa o Tipo de Requisição no padrão do Moses
        /// </summary>
        public enum MosesRequestType
        {
            //Request em página de login
            LoginPage,
            //Request em página institucional (Anonimo)
            SitePage,
            //Request em página autenticada
            Page

        }

        [HandleMosesAppException]
        public class MosesController : Controller
        {
            public MosesController()
            {
                
            }

            protected override void Initialize(System.Web.Routing.RequestContext requestContext)
            {
                base.Initialize(requestContext);
                MembershipContext = MembershipContextFactory.Initialize(requestContext.HttpContext);
            }

            public IFormsAuthentication FormsAuth { get; set; }

            public Guid? UserId { get { if (MembershipContext.User == null) return null; return this.MembershipContext.User.Id; } }
            public int? ContractId { get { if (MembershipContext.Contract == null) return null; return this.MembershipContext.Contract.Id; } }
            public string ContractName { get { return MembershipContext.Contract.Id.ToString(); } }

            public string ErrorMessage
            {
                get
                {
                    return TempData["__ErrorMessage"] as string;
                }
                set
                {
                    TempData["__ErrorMessage"] = value;
                }
            }
            public string InfoMessage
            {
                get
                {
                    return TempData["__InfoMessage"] as string;
                }
                set
                {
                    TempData["__InfoMessage"] = value;
                }
            }

            public List<string> Roles { get; set; }

            private List<MembershipRole> AuthorizatedRoles
            {
                get
                {
                    MembershipRoleManager mgr = new MembershipRoleManager(MembershipContext);
                    return mgr.GetAvailableRoles();
                }
            }

            //private bool isAuthorized
            //{
            //    get
            //    {
            //        if (Roles == null) //Roles definidas na requisição. null = não possui restrições
            //            return true;

            //        if (MembershipContext.HasPermission(AuthorizatedRoles.Select(q => q.Name).ToArray()))
            //            return true;

            //        return false;
            //    }
            //}

            public bool IsLoginPage
            {
                get { return MosesRequestType.LoginPage == this.PageType; }
            }

            public bool IsSitePage
            {
                get { return MosesRequestType.SitePage == this.PageType; }
            }

            public virtual MosesRequestType PageType
            {
                get { return MosesRequestType.Page; }
            }

            protected override void OnAuthorization(AuthorizationContext filterContext)
            {
                //se não for a página de login e não for página institucional
                if (!this.IsLoginPage && !this.IsSitePage)
                {
                    //se o userContext estiver null, redireciona para o login
                    if (!HasUser)// || !isAuthorized)
                    {
                        var loginUrl = Configuration.LoginUrl;
                        if (Request.IsAjaxRequest())
                        {
                            //redireciona para o login
                            filterContext.Result = Json(new { relogin = true, message = "Sua sessão expirou. Recarregue o navegador (digite F5) e faça o login.", success = false });
                        }
                        else
                        {
                            filterContext.Result = Redirect(loginUrl);
                        }
                    }
                    else //se não for página de login ou institucional e tiver user autorizado
                    {
                        //não faz nada
                    }
                }

                base.OnAuthorization(filterContext);
            }


            public string UserName
            {
                get { if (this.HasUser) { return MembershipContext.User.FullName; } return ""; }
            }

            public bool HasUser
            {
                get
                {
                    return this.MembershipContext.User != null;
                }
            }
            
            public MembershipContext MembershipContext { get; set; }
            public IMembershipManager Membership { get; set; }

            protected override JsonResult Json(object data, string contentType, Encoding contentEncoding)
            {
                return new JsonNetResult()
                {
                    Data = data,
                    ContentType = contentType,
                    ContentEncoding = contentEncoding,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            #region IMosesPermissionContainer Members

            MosesPermissionContainer _permissionContainer;

            public IMosesPermission MosesPermission
            {
                get
                {

                    if (_permissionContainer == null)
                    {
                        _permissionContainer = new MosesPermissionContainer(MembershipContext);
                    }
                    return _permissionContainer.GetPermissionSet();
                }
            }

            public void Update()
            {
                if (_permissionContainer != null)
                {
                    _permissionContainer._updatePermissionsExecuted = true;
                }
            }

            #endregion

            public JsonResult Fail(string message,  string defaultFailBehavior = null, object misc = null, long? index = null, Exception exception = null)
            {
                return new JsonNetResult
                {
                    Data = ResponseViewModel.ApplyFail(message, defaultFailBehavior, misc, index, false, exception),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            public JsonResult Success(string message,  string defaultSuccessBehavior = null, object misc = null, long? index = null)
            {
                return new JsonNetResult
                {
                    Data = ResponseViewModel.ApplySuccess(message, defaultSuccessBehavior, misc, index, false),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            public JsonResult Saved(string message, dynamic model = null,  object misc = null, MosesOperation? operation = null)
            {
                return ResponseViewModel.SaveResult(message, model,  misc, operation: operation);
            }

            public JsonResult Deleted(string message, dynamic model,  object misc = null, MosesOperation? operation = MosesOperation.Reload)
            {
                return ResponseViewModel.DeleteResult(message, model,  misc, operation: operation);
            }

            public PartialViewResult PartialError(string message, string title = "Operação não suportada", string subTitle = "Uma tentativa de operação inválida ocorreu", dynamic model = null, bool hideButton = false)
            {
                ViewData["message"] = message;
                ViewData["title"] = title;
                ViewData["subTitle"] = subTitle;
                ViewBag.HideButton = hideButton;
                return PartialView("Alert", model);
            }

            protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
            {
                return new JsonNetResult
                {
                    Data = data,
                    ContentType = contentType,
                    ContentEncoding = contentEncoding,
                    JsonRequestBehavior = behavior
                };
            }


        }



}
