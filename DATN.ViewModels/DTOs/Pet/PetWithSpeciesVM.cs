using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Pet
{
	public class PetWithSpeciesVM
	{
		public int Id { get; set; } // Khóa chính
		public Guid OwnerId { get; set; } // Khóa ngoại đến ID chủ nhân
		public string NameOwner { get; set; } // Khóa ngoại đến ID chủ nhân
		public string Name { get; set; } // Tên thú cưng
		public bool Gender { get; set; } // Giới tính

	}
}
