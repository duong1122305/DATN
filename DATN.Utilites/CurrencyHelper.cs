using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Utilites
{
    public static class CurrencyHelper
    {
        public static string FormatCurrency(float price)
        {
            CultureInfo vnCultureInfo = new CultureInfo("vi-VN");
            string formattedAmount = price.ToString("N0", vnCultureInfo);
            return formattedAmount + " VND";
        }
    }
}
