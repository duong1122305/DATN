using DATN.Utilites.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Guest
{
    public class GuestChangPassRequest
    {
        public string UserName { get; set; }
        [PasswordValidation]
        public string OldPass { get; set; }
        [PasswordValidation]
        public string NewPass { get; set; }
    }
}
