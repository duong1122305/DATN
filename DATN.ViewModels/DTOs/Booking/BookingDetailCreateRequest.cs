using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Booking
{
	public class BookingDetailCreateRequest
	{
		public int? ServiceDetailId { get; set; } // Khóa ngoại đến ID chi tiết dịch vụ
		public int Quantity { get; set; } // Khóa ngoại đến ID chi tiết dịch vụ
		public Guid StaffId { get; set; } // Khóa ngoại đến ID chi tiết dịch vụ
		public DateTime StartDateTime { get; set; } // Thời gian bắt đầu
		public DateTime EndDateTime { get; set; } // Thời gian kết thúc
		public int? ComboId { get; set; }
	}
}
