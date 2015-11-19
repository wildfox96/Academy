using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUPPRB.Common.Extensions
{
    public  static class DateTimeExtensions
    {
        public static int WeeksInYear(this DateTime date)
        {
            var cal = new GregorianCalendar(GregorianCalendarTypes.Localized);
            return cal.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff =startOfWeek- dt.DayOfWeek  ;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(1 * diff).Date;
        }
    }
}
