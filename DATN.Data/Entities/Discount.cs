﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Entities
{
	// Bảng Giảm giá
	[Table("Discounts")]
	public class Discount
	{
		public int Id { get; set; } // Khóa chính

		public int Quantity { get; set; } // Số lượng

		public string VoucherCode { get; set; } // Mã giảm giá

		public string Description { get; set; } // Mô tả

		public DateTime Created { get; set; } // Thời gian tạo

		public DateTime StartDate { get; set; } // Ngày bắt đầu

		public DateTime EndDate { get; set; } // Ngày kết thúc

		public decimal DiscountPercent { get; set; } // Phần trăm giảm giá

		public double MaxMoneyDiscount { get; set; } // Giới hạn tiền giảm tối đa

		public double MinMoneyApplicable { get; set; } // Giá trị tối thiểu áp dụng giảm giá

		public DateTime? DeleteAt { get; set; } // Thời gian xóa
		public virtual ICollection<Booking> Booking { get; set; }
	}
}