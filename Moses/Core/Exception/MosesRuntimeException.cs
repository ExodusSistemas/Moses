using System;
using System.Collections.Generic;
using System.Text;

namespace Moses
{
    public class MosesRuntimeException : MosesException
    {
        public MosesRuntimeException(string message, System.Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
