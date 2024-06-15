﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Entities
{
	// Bảng Thú cưng
	[Table("Pets")]
	public class Pet
	{
		public int Id { get; set; } // Khóa chính

		public Guid OwnerId { get; set; } // Khóa ngoại đến ID chủ nhân

		public int SpeciesId { get; set; } // Khóa ngoại đến ID giống thú cưng

		public string Name { get; set; } // Tên thú cưng

		public bool Gender { get; set; } // Giới tính

		public DateTime Birthday { get; set; } // Ngày sinh

		public float Weight { get; set; } // Cân nặng

		public bool Neutered { get; set; } // Đã triệt sản chưa

		public string OriginalColor { get; set; } // Màu lông ban đầu

		public bool Vaccinated { get; set; } // Đã tiêm phòng chưa

		public string Note { get; set; } // Ghi chú

		public virtual Guest? Guest { get; set; } // Chủ nhân

		public virtual PetSpecies Species { get; set; } // Giống thú cưng
	}
}
