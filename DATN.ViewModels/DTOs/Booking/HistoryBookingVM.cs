using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Booking
{
	public class HistoryBookingVM
	{
		public int ID { get; set; }
		public string TimeAction { get; set; }
		public string ActionName { get; set; }
		public string ActionBy { get; set; }
	}
}
