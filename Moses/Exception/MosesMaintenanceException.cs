using System;
using System.Collections.Generic;
using System.Text;

namespace Moses
{
    public class MosesMaintenanceException : MosesException
    {
        public MosesMaintenanceException(string message, System.Exception innerException) : base(message,innerException)
        {

        }
    }
}
