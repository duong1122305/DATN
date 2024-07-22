namespace DATN.Utilites.Check
{
    public static class CheckServiceDetail
    {
        public static bool CheckPriceIsFormat(float price)
        {
            if (price <= 0 && price.ToString().StartsWith("0"))
            {
                return false;
            }
            return true;
        }

        public static bool CheckLengthServiceName(string name)
        {
            if (string.IsNullOrEmpty(name) && name.Length == 0 && name.Length > 100)
            {
                return false;
            }

            return true;
        }
    }
}
