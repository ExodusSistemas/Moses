namespace Moses.Web.Mvc.Controls
{
    using System;

    public static class Guard
    {
        public static void IsNotNull(object parameter, string parameterName)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName, " cannot be null.");
            }
        }

        public static void IsNotNull(object parameter, string parameterName, string message)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName, " " + message);
            }
        }

        public static void IsNotNullOrEmpty(string parameter, string parameterName)
        {
            if (string.IsNullOrEmpty(parameter))
            {
                throw new ArgumentNullException(parameterName, " cannot be null or empty.");
            }
        }

        public static void IsNotNullOrEmpty(string parameter, string parameterName, string message)
        {
            if (string.IsNullOrEmpty(parameter))
            {
                throw new ArgumentNullException(parameterName, " " + message);
            }
        }
    }
}

