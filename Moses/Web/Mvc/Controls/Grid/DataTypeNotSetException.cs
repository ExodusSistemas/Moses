namespace Moses.Web.Mvc.Controls
{
    using System;

    internal class DataTypeNotSetException : Exception
    {
        public DataTypeNotSetException(string message) : base(message)
        {
        }
    }
}

