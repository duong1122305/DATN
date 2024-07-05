using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.Enum
{
	public enum BookingDetailStatus
	{
		IsActive,
        Unfulfilled,
        Processing,// Đã hoàn thành
		Cancelled       // Đã huỷ
	}

}
