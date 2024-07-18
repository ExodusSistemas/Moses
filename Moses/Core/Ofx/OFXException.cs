using System;
using System.Runtime.Serialization;

namespace Moses.Ofx
{
    [Serializable]
    public class OFXException : Exception
    {
        public OFXException()
        {
        }

        public OFXException(string message) : base(message)
        {
        }

        public OFXException(string message, Exception inner) : base(message, inner)
        {
        }

    }
}