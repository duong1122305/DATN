﻿using System.ComponentModel.DataAnnotations.Schema;

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

        public DateTime? Birthday { get; set; } // Ngày sinh

        public float? Weight { get; set; } // Cân nặng

        public bool? Neutered { get; set; } // Đã triệt sản chưa

        public bool? IsDelete { get; set; } // Đã tiêm phòng chưa

        public string? Note { get; set; } // Ghi chú

        public virtual Guest? Guest { get; set; } // Chủ nhân
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }

        public virtual PetSpecies Species { get; set; } // Giống thú cưng
    }
}
