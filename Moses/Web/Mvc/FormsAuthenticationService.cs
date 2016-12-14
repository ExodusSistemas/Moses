using System;
using System.Text;
using System.Web.Security;
using System.Security.Cryptography;

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

        public string HashPasswordForStoringInConfigFile(string password, string passwordFormat)
        {
            HashAlgorithm algorithm = HashAlgorithm.Create(passwordFormat);
            if (algorithm == null)
            {
                throw new ArgumentException("Unrecognized hash name", "hashName");
            }
            byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hash);
        }

        #endregion
    }

    
}
