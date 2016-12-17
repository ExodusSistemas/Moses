namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;

    internal static class StringBuilderExtensions
    {
        public static void AppendFormatIfFalse(this StringBuilder sb, bool parameter, string value, params object[] args)
        {
            if (!parameter)
            {
                sb.AppendFormat(value, args);
            }
        }

        public static void AppendFormatIfNotNull(this StringBuilder sb, object parameter, string value, params object[] args)
        {
            if (parameter != null)
            {
                sb.AppendFormat(value, args);
            }
        }

        public static void AppendFormatIfNotNullOrEmpty(this StringBuilder sb, string parameter, string value)
        {
            if (!string.IsNullOrEmpty(parameter))
            {
                sb.AppendFormat(value, parameter);
            }
        }

        public static void AppendFormatIfNotNullOrEmpty(this StringBuilder sb, string parameter, string value, params object[] args)
        {
            if (!string.IsNullOrEmpty(parameter))
            {
                sb.AppendFormat(value, args);
            }
        }

        public static void AppendFormatIfTrue(this StringBuilder sb, bool parameter, string value, params object[] args)
        {
            if (parameter)
            {
                sb.AppendFormat(value, args);
            }
        }

        public static void AppendIfFalse(this StringBuilder sb, bool parameter, string value)
        {
            if (!parameter)
            {
                sb.Append(value);
            }
        }

        public static void AppendIfNotNullOrEmpty(this StringBuilder sb, string parameter, string value)
        {
            if (!string.IsNullOrEmpty(parameter))
            {
                sb.Append(value);
            }
        }

        public static void AppendIfTrue(this StringBuilder sb, bool parameter, string value)
        {
            if (parameter)
            {
                sb.Append(value);
            }
        }
    }
}

