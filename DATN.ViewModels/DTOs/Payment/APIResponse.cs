using DATN.ViewModels.DTOs.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Payment
{
    public class APIResponse
    {
        public string code { get; set; }
        public string desc { get; set; }
        public DataPayment data { get; set; }

    }
}
