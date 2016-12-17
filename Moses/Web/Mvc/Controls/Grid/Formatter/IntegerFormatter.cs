namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Runtime.CompilerServices;

    public class IntegerFormatter : Moses.Web.Mvc.Controls.JQGridColumnFormatter
    {
        public string DefaultValue { get; set; }

        public string ThousandsSeparator { get; set; }
    }
}

