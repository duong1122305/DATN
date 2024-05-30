using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Extentions
{
    public class RandomCodeExtention
    {
        public string RandomCode()
        {
            string code = "";
            for (int i = 0; i < 6; i++)
            {
                Random random =new Random();
                if (random.Next(0, 10) % 2 == 0)
                {
                    code += random.Next(0, 9).ToString();
                }
                else code += Convert.ToChar(random.Next(65, 90)).ToString();
            }
            return code;
        }
    }
}
