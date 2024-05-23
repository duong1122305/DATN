using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Entities
{
	// Bảng Loại thanh toán
	[Table("TypePayments")]
	public class TypePayment
	{
		[Key]
		public int Id { get; set; } // Khóa chính

		[Required]
		public string Name { get; set; } // Tên loại thanh toán
	}
}
