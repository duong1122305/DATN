using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Guest
{
    public class GetGuestRequest
    {
        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string? keyWord {  get; set; }
    }
}
