using System;
using System.Collections.Generic;
using System.Text;

namespace Moses
{
    public abstract class MosesException : System.ApplicationException
    {
        public MosesException()
            : base()
        { 
        }

        public MosesException(string message) : base(message)
        {

        }

        public MosesException(string message, System.Exception innerException)
            : base(message, innerException)
        {
            
        }


    }

    public class MosesDataProviderException : MosesException
    {
        public string SqlCommand;

        public MosesDataProviderException(string message)
            : this(message,null,null)
        {
        }

        public MosesDataProviderException(string message, string sqlCommand,System.Exception innerException) :
            base(String.Format("{0}\n Comando Sql:{1} \n Driver: {2}",message,sqlCommand,innerException.Message))
        {
            
        }
    }

    public class MosesSqlGenerationException : MosesException
    {
        public MosesSqlGenerationException(string message)
            : base(message)
        {

        }
    }

    public class MosesConfigurationException : MosesException
    {
        public MosesConfigurationException(string message)
            : base(message)
        {
        }
        
    }

    public class MosesFormatException : MosesException
    {
        public MosesFormatException(string message)
            : base(message)
        {
        }

    }

    public class MosesValidationException : MosesException
    {
        public MosesValidationException(string message)
            : base(message)
        {
        }

    }

    public class MosesPaserException : MosesException
    {
        public MosesPaserException(string message)
            : base(message)
        {
        }

    }

    public enum MosesErrorProviderOptions
    {
        Email,
        XmlLogFile,
        Both
    }
}
