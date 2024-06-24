using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Authenticate
{
    public class UserResetPassView
    {
        public string UserName { get; set; }
        public string NewPassWord { get; set; }
        public string ConfirmPassWord { get; set; }
    }
}
