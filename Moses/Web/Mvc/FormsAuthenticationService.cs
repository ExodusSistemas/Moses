using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using Moses.Extensions;

namespace Moses.Web.Mvc
{
    public class FormsAuthenticationService : IFormsAuthentication
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            //userName.ValidateRequired("userName");
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }


        public void SetAuthCookie(string userName, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }
            
        #region IFormsAuthentication Members


        public string HashPasswordForStoringInConfigFile(string oldPassword, string p)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(oldPassword,p);
        }

        #endregion
    }

    
}
