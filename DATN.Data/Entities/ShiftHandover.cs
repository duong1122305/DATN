using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Entities
{
	// Bảng Bàn giao ca nhân viên quầy
	[Table("ShiftHandovers")]
	public class ShiftHandover
	{
		[Key]
		public int Id { get; set; } // Khóa chính

		[ForeignKey("User")]
		public string UserId { get; set; } // Khóa ngoại đến ID nhân viên

		public DateTime CheckInTime { get; set; } // Thời gian vào

		public DateTime CheckOutTime { get; set; } // Thời gian ra

		public double InitialCash { get; set; } // Tiền mặt ban đầu

		public double InitialAccountBalance { get; set; } // Số dư tài khoản ban đầu

		public double FinalCash { get; set; } // Tiền mặt cuối ca

		public double FinalAccountBalance { get; set; } // Số dư tài khoản cuối ca

		public double TotalEarnings { get; set; } // Tổng thu nhập

		public double TotalOtherExpenses { get; set; } // Tổng chi phí khác

		public double TotalOtherIncomes { get; set; } // Tổng thu khác

		public string OtherExpenseReason { get; set; } // Lý do chi phí khác

		public string OtherIncomeReason { get; set; } // Lý do thu nhập khác

		public string OtherNotes { get; set; } // Ghi chú khác

		public virtual User User { get; set; } // Nhân viên
	}
}
