using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Enum
{
	public enum BookingStatus
	{
		PENDING_CONFIRMATION, // Chờ xác nhận
		CONFIRMED,            // Đã xác nhận
		ARRIVED,              // Đã đến nơi
		IN_PROGRESS,          // Đang thực hiện
		COMPLETED,            // Đã hoàn thành
		MISSED,              // Khách không đến
		CUSTOMER_CANCELLED,   // Khách huỷ
		STAFF_CANCELLED,      // Nhân viên huỷ
		ADMIN_CANCELLED       // Admin huỷ
	}
}
