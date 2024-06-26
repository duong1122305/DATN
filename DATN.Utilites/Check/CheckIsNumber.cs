using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Utilites.Check
{
    public static class CheckIsNumber
    {
        public static bool Check(string number)
        {
            if(double.TryParse(number, out _))
            {
                return true;
            }
            return false;
        }
    }
}
