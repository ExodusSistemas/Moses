namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Runtime.CompilerServices;

    public class NumberFormatter : Moses.Web.Mvc.Controls.JQGridColumnFormatter
    {
        public int DecimalPlaces { get; set; }

        public string DecimalSeparator { get; set; }

        public string DefaultValue { get; set; }

        public string ThousandsSeparator { get; set; }
    }
}

