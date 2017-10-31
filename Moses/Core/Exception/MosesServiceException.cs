using System;
using System.Collections.Generic;
using System.Text;

namespace Moses.Core.Exception
{
    public class MosesServiceException : MosesException
    {
        
        public MosesServiceException(string message)
            : base(message)
        {

        }

        public MosesServiceException(string message, System.Exception innerException)
            : base(message, innerException)
        {

        }
        
    }
}
