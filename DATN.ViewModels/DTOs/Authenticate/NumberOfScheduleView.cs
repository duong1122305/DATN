using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Authenticate
{
    public class NumberOfScheduleView
    {
        public int shiftId { get; set; }
        public string ShiftName { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public DateTime Date { get; set; }
    }
}
