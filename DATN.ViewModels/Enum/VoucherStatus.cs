using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.Enum
{
    public enum VoucherStatus
    {
        Expired = 0, //hết hạn 
        GoingOn = 1, //còn hạn
        NotOccur = 2 //chưa bắt đầu
    }
}
