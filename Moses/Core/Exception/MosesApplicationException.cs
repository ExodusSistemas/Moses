using System;
using System.Collections.Generic;
using System.Text;

namespace Moses
{
    public class MosesApplicationException : MosesException
    {
        public MosesApplicationException(string message)
            : base(message)
        {

        }

        public MosesApplicationException(string message, System.Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
