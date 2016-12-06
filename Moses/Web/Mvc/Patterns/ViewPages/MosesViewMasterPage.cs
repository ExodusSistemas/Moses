using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Mvc;
using System.Text;
using Moses.Extensions;
using Moses;


namespace Moses.Web.Mvc.Patterns
{

    public class MosesWebViewMasterPage : WebViewPage, IMosesPermissionContainer
    {
        public MembershipContext MembershipContext { get; set; }

        public MosesWebViewMasterPage()
        {
            MembershipContext = MembershipContextFactory.Initialize(new HttpContextWrapper(HttpContext.Current));
        }

        public override void Execute()
        {

        }

        public int? WorkingContractId
        {
            get
            {
                return MembershipContext.Contract.Id;
            }
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
    }

    public static class MosesMasterPageHelper
    {
        public static Dictionary<int, string> GetContracts(this IMosesPermissionContainer container)
        {
            if (container.MembershipContext.AvailableContractsList != null)
            {
                string s = container.MembershipContext.AvailableContractsList as string;

                Dictionary<int, string> inst = new Dictionary<int, string>();

                foreach (var m in s.Split(';').Where(q => q.Contains(':')))
                {
                    var a = m.Split(':');
                    inst.Add(int.Parse(a[0]), a[1]);
                }
                return inst;
            }
            else return new Dictionary<int, string>();
        }

        //Se o serviço for trial
        public static bool IsTrial(this IMosesPermissionContainer container)
        {
            return false;
        }

        public static bool HasTopMessage(this IMosesPermissionContainer container)
        {
            //if (container.MembershipContext.Contract.Suspenso.HasValue)
            //    return container.MembershipContext.Contract.Suspenso.Value;

            //if (container.MembershipContext.Contract.IsEvaluation.HasValue)
            //    return container.MembershipContext.Contract.IsEvaluation.Value;

            return false;
        }

        //Verifica data de Expiração
        public static DateTime GetExpirationDate(this IMosesPermissionContainer container)
        {
            //TODO Desenvolver uma forma eficiente de verificar que a assinatura esta em dia
            //if (container.MembershipContext.Contract.DatExpiracao.HasValue)
            //    return container.MembershipContext.Contract.DatExpiracao.Value;

            return DateTime.Now.AddDays(99);
        }

        public static void OnLoadMasterPages(this Control masterPage)
        {
            masterPage.Page.Title = Configuration.DefaultTitle ?? "Sistema Online";
        }
    }


}
