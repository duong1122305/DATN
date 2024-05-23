using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Entities
{
	// Bảng Dịch vụ
	[Table("Services")]
	public class Service
	{
		[Key]
		public int Id { get; set; } // Khóa chính

		[Required]
		public string Name { get; set; } // Tên dịch vụ

		// Quan hệ một-nhiều: Mỗi dịch vụ có thể được bán trong nhiều gói dịch vụ (combo)
		public virtual ICollection<ComboDetail> ComboDetails { get; set; }
	}
}
