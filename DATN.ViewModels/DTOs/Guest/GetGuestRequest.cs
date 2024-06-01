using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Guest
{
    public class GetGuestRequest
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public string? keyWord {  get; set; }
    }
}
