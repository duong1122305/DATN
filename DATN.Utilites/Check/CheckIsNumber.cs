namespace DATN.Utilites.Check
{
    public static class CheckIsNumber
    {
        public static bool Check(string number)
        {
            if (double.TryParse(number, out _))
            {
                return true;
            }

            if (float.TryParse(number, out _))
            {
                return true;
            }

            return false;
        }
    }
}
