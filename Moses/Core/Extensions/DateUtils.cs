using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Moses.Extensions
{
    public static class DateUtils
    {
        public static DateTime GetFirstDateOfWeek(this DateTime dayInWeek)
        {
            CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;
            return GetFirstDateOfWeek(dayInWeek, defaultCultureInfo);
        }

        public static DateTime GetFirstDateOfWeek(this DateTime dayInWeek, CultureInfo cultureInfo)
        {
            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            DayOfWeek selDow = cultureInfo.Calendar.GetDayOfWeek(dayInWeek);

            int diff = selDow - firstDay;

            return dayInWeek.AddDays(-diff);
        }

        public static string ToExtensiveFormat(this DateTime date, CultureInfo cultureInfo)
        {
            return date.ToString("dd 'de' MMMM 'de' yyyy", cultureInfo);
        }

        public static string ToExtensiveFormat(this DateTime date)
        {
            return date.ToExtensiveFormat(System.Globalization.CultureInfo.GetCultureInfo("pt-BR"));
        }
    }
}
