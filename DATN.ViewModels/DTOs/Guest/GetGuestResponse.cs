using DATN.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Guest
{
    public class GetGuestResponse
    {
        public PagingResponseData PagingData { get; set; }
        public List<GuestViewModel> lstGuest { get; set; }
    }
}
