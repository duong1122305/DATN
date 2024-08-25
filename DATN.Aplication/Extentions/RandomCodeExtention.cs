namespace DATN.Aplication.Extentions
{
    public static class RandomCodeExtention
    {
        public static string RandomCode()
        {
            string code = "";
            for (int i = 0; i < 6; i++)
            {
                Random random = new Random();
                if (random.Next(0, 10) % 2 == 0)
                {
                    code += random.Next(0, 9).ToString();
                }
                else code += Convert.ToChar(random.Next(65, 90)).ToString();
            }
            return code;
        }
        public static string RandomCodeOnlyNumber()
        {
            string code = "";
            for (int i = 0; i < 6; i++)
            {
                Random random = new Random();
                code += random.Next(0, 9).ToString();
            }
            return code;
        }
        public static string GennarateVerifyCode(string ID)
        {
            Guid randomGuid = Guid.NewGuid();

            string verifyString = ID + "|" + DateTime.Now.AddMinutes(3).ToString();
            PasswordExtensitons hasCode = new PasswordExtensitons();
            string verifyCode = hasCode.HashCode(verifyString);
            return verifyCode;
        }
    }
}
