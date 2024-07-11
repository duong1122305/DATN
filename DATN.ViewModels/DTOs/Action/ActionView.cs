using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Action
{
    public class ActionView
    {
        public int IdBokingOrDetail { get; set; }
        public string? Token { get; set; }
        public string? Reason { get; set; }
    }
}
