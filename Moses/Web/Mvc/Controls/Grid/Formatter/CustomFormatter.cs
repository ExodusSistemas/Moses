namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Runtime.CompilerServices;

    public class CustomFormatter : Moses.Web.Mvc.Controls.JQGridColumnFormatter
    {
        public string FormatFunction { get; set; }

        public string UnFormatFunction { get; set; }
    }
}

