using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Entities
{
	// Bảng Loại thú cưng
	[Table("PetTypes")]
	public class PetType
	{
		[Key]
		public int Id { get; set; } // Khóa chính

		[Required]
		public string Name { get; set; } // Tên loại thú cưng

		// Quan hệ một-nhiều: Mỗi loại thú cưng có thể có nhiều giống thú cưng
		public virtual ICollection<PetSpecies> Species { get; set; }
	}
}
