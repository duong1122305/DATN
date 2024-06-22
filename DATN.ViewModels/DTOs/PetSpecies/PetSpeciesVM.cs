using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.PetSpecies
{
	public class PetSpeciesVM
	{
		public int Id { get; set; }
		public int PetTypeId { get; set; } // Khóa ngoại đến ID loại thú cưng
		public string PetPype {  get; set; }	
		public string Name { get; set; } // Tên giống thú cưng
		public bool? IsDelete { get; set; } = false;// Xoá mềm loài
		public int? CountPet { get; set; } = 0;
	}
}
