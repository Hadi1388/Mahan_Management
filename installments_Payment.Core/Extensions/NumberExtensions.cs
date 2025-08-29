using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace installments_Payment.Core.Extensions
{
    public static class NumberExtensions
    {
        public static string ToSeparatedNumber(this int number)
        {
            return number.ToString("N0");  // فرمت عدد با جداکننده هزارگان بدون اعشار
        }

        public static string ToSeparatedNumber(this decimal number)
        {
            return number.ToString("N0");  // برای اعداد اعشاری هم قابل استفاده است
        }

        public static string ToSeparatedNumber(this long number)
        {
            return number.ToString("N0");
        }
    }
}
