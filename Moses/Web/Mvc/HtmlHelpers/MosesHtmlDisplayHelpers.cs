using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace Moses.Web.Mvc.Html
{
    public static class MosesHtmlDisplayHelpers
    {
        private static readonly string _moneyStringFormat = "{0:#,##0.00;(#,##0.00);-}";
        private static readonly string _moneySymbolStringFormat = "R$ {0:#,##0.00;(#,##0.00);-}";


        public static string DisplayPercentVariation(this decimal number, decimal? pivot = null, bool progressive = true, bool showSymbol = true)
        {
            var symbol = "";
            if (showSymbol) symbol = "%";


            decimal variation = 0;
            string variationText = "";
            string colorClass = "";

            if (pivot == null)
            {
                variation = number;
            }
            else  if( number > pivot){
                variation = CalculateVariacao(pivot.GetValueOrDefault(), number );
            }

            if ( variation == 0){
                variationText = "";
            }
            else if (variation > 0) {
                variationText = "fa-level-up";
                colorClass= progressive? "text-navy" : "text-danger";

            }
            else{
                variationText = "fa-level-down";
                colorClass = progressive ? "text-danger" : "text-navy";
            }

            return string.Format(@"<div class='{1}' ><i class='fa {2}' ></i>{0}</div>", variation.DisplayPercent(showSymbol), colorClass, variationText );
        }

        public static decimal CalculateVariacao(decimal p1, decimal p2)
        {
            if (p1 > 0)
                return  (p2 - p1) / p1;
            else if (p1 < 0)
                return  -(p2 - p1) / p1;
            else if (p1 == 0 && p2 != 0)
                return 1;
            return 0;
        }

        public static string DisplayPercent(this decimal number, bool showSymbol = true )
        {
            var symbol = "";
            if(showSymbol) symbol = "%";
            return string.Format("{0:n2}{1}", number*100, symbol);
        }

        public static string DisplayPercentOrDefault(this decimal? percent, bool showSymbol = true)
        {
            return percent.GetValueOrDefault().DisplayPercent(showSymbol);
        }

        public static string DisplayMoney(this decimal money, bool showSymbol = true, CultureInfo info = null )
        {
            if ( showSymbol )
                return String.Format(_moneySymbolStringFormat, money);
            else
                return String.Format(_moneyStringFormat, money);
        }

        public static string DisplayMoneyOrDefault(this decimal? money, bool showSymbol = true)
        {
            return money.GetValueOrDefault().DisplayMoney(showSymbol);
        }

        public static string DisplayDate(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }

        public static string DisplayMonth(this DateTime date)
        {
            return date.ToString("MM/yyyy");
        }

        public static string FromToday(this DateTime? date , string nullExpression = "Nunca")
        {
            if (date.HasValue)
            {
                return FromToday(date.Value);
            }

            return nullExpression;
        }

        public static string FromToday(this DateTime date)
        {
            // 1.
            // Get time span elapsed since the date.
            TimeSpan s = DateTime.Now.Subtract(date);

            return FromTimeSpan(s);
        }

        public static string FromTimeSpan(this TimeSpan s, DateTime? originalDate = null)
        {

            // 2.
            // Get total number of days elapsed.
            int dayDiff = (int)s.TotalDays;

            // 3.
            // Get total number of seconds elapsed.
            int secDiff = (int)s.TotalSeconds;

            // 4.
            // Don't allow out of range values.
            if (dayDiff < 0 )
            {
                return null;   
            }

            if (dayDiff >= 31)
            {
                var res = Math.Ceiling((double)dayDiff / 30);
                var meses = res > 1 ? "meses" : "mês";
                return string.Format("Há mais de {0} {1}", res, meses);//esta conta não está tão bem arredondada, mas ainda assim é uma tautologia.
            }

            // 5.
            // Handle same-day times.
            if (dayDiff == 0)
            {
                // A.
                // Less than one minute ago.
                if (secDiff < 60)
                {
                    return "neste minuto";
                }
                // B.
                // Less than 2 minutes ago.
                if (secDiff < 120)
                {
                    return "1 minuto atrás";
                }
                // C.
                // Less than one hour ago.
                if (secDiff < 3600)
                {
                    return string.Format("{0} minutos atrás",
                        Math.Floor((double)secDiff / 60));
                }
                // D.
                // Less than 2 hours ago.
                if (secDiff < 7200)
                {
                    return "1 hora atrás";
                }
                // E.
                // Less than one day ago.
                if (secDiff < 86400)
                {
                    return string.Format("{0} horas atrás",
                        Math.Floor((double)secDiff / 3600));
                }
            }
            // 6.
            // Handle previous days.
            if (dayDiff == 1)
            {
                return "ontem";
            }
            if (dayDiff < 7)
            {
                return string.Format("{0} dias atrás",
                dayDiff);
            }
            if (dayDiff < 31)
            {
                return string.Format("{0} semanas atrás",
                Math.Ceiling((double)dayDiff / 7));
            }

            if (originalDate.HasValue)
            {
                return DisplayDate(originalDate.Value);
            }
            else
            {
                return string.Format("Mais de {0} semanas atrás",
                Math.Ceiling((double)dayDiff / 7));
            }
        }

        public static string DisplayDateOrDefault(this DateTime? date)
        {
            return date == null ? "" : date.Value.ToString("dd/MM/yyyy");
        }

        public static string DisplayMonthOrDefault(this DateTime? date)
        {
            return date == null ? "" : date.Value.ToString("MM/yyyy");
        }



    }
}
