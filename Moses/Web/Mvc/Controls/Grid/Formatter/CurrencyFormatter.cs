namespace Moses.Web.Mvc.Controls
{
    using System;
    using System.Runtime.CompilerServices;

    public class CurrencyFormatter : Moses.Web.Mvc.Controls.JQGridColumnFormatter
    {
        public int DecimalPlaces { get; set; }

        public string DecimalSeparator { get; set; }

        public string DefaultValue { get; set; }

        public string Prefix { get; set; }

        public string Suffix { get; set; }

        public string ThousandsSeparator { get; set; }
    }
}

