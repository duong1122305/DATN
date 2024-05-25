using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.Enum
{
	public enum BookingDetailStatus
	{
		Pending,        // Chờ xử lý
		Confirmed,      // Đã xác nhận
		InProgress,     // Đang thực hiện
		Completed,      // Đã hoàn thành
		Cancelled       // Đã huỷ
	}

}
