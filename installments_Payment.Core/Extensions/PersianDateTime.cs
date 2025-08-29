using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace installments_Payment.Core.Extensions
{
    public class PersianDateTime
    {
        public static string ToPersianDateTimeString(DateTime dt)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            int year = pc.GetYear(dt);
            int month = pc.GetMonth(dt);
            int day = pc.GetDayOfMonth(dt);
            int hour = pc.GetHour(dt);
            int minute = pc.GetMinute(dt);
            return $"{year:0000}/{month:00}/{day:00} {hour:00}:{minute:00}";
        }
    }
}
