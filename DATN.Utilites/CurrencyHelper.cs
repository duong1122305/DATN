using System.Globalization;

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
