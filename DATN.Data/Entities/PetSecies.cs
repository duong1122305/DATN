using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Entities
{
	// Bảng Giống thú cưng
	[Table("PetSpecies")]
	public class PetSpecies
	{
		public int Id { get; set; } // Khóa chính

		public int PetTypeId { get; set; } // Khóa ngoại đến ID loại thú cưng

		public string Name { get; set; } // Tên giống thú cưng

		public string Type { get; set; } // Loại (nếu cần)

		public virtual PetType PetType { get; set; } // Loại thú cưng
		public virtual ICollection<Pet> Pets { get; set; } // Loại thú cưng
	}
}
