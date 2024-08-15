using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Booking
{
	public class StatiscalBookingDetail
	{
        public int ID { get; set; }
		public string ServiceName { get; set; }
        public string Amount { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string StaffName { get; set; }
        public string Status { get; set; }
    }
}
