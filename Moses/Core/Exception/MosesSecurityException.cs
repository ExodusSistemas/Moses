using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moses.Security
{
    public class MosesSecurityException : Moses.MosesApplicationException
    {
        public MosesSecurityException(string message, Exception innerException) : base(message, innerException)
        {

        }


    }
}
