using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Moses.Extensions
{
    
    public static class MosesSecurityHelper
    {
        [Obsolete]
        public static void RegenerateSessionId(this Controller controller)
        {
            System.Web.SessionState.SessionIDManager manager = new System.Web.SessionState.SessionIDManager();
            string oldId = manager.GetSessionID(controller.HttpContext.ApplicationInstance.Context);
            string newId = manager.CreateSessionID(controller.HttpContext.ApplicationInstance.Context);
            bool isAdd = false, isRedir = false;
            manager.SaveSessionID(controller.HttpContext.ApplicationInstance.Context, newId, out isRedir, out isAdd);
            HttpApplication ctx = (HttpApplication)controller.HttpContext.ApplicationInstance.Context.ApplicationInstance;
            HttpModuleCollection mods = ctx.Modules;
            System.Web.SessionState.SessionStateModule ssm = (SessionStateModule)mods.Get("Session");
            System.Reflection.FieldInfo[] fields = ssm.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            SessionStateStoreProviderBase store = null;
            System.Reflection.FieldInfo rqIdField = null, rqLockIdField = null, rqStateNotFoundField = null;
            foreach (System.Reflection.FieldInfo field in fields)
            {
                if (field.Name.Equals("_store")) store = (SessionStateStoreProviderBase)field.GetValue(ssm);
                if (field.Name.Equals("_rqId")) rqIdField = field;
                if (field.Name.Equals("_rqLockId")) rqLockIdField = field;
                if (field.Name.Equals("_rqSessionStateNotFound")) rqStateNotFoundField = field;
            }
            object lockId = rqLockIdField.GetValue(ssm);
            if ((lockId != null) && (oldId != null)) store.ReleaseItemExclusive(controller.HttpContext.ApplicationInstance.Context, oldId, lockId);
            rqStateNotFoundField.SetValue(ssm, true);
            rqIdField.SetValue(ssm, newId);
        }
       
    }
}
