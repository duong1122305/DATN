﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.PetSpecies
{
	public class PetSpeciesCreateUpdate
	{

		public int Id { get; set; }
		[Required(ErrorMessage ="Phải chọn loài động vật")]
		public int PetTypeId { get; set; } // Khóa ngoại đến ID loại thú cưng
		[Required(ErrorMessage = "Phải chọn tên thú cưng")]
		public string Name { get; set; } // Tên giống thú cưng
		public bool? IsDelete { get; set; } = false;// Xoá mềm loài
	}
}