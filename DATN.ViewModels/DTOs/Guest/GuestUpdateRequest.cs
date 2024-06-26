﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Guest
{
    public class GuestUpdateRequest
    {
        public Guid Id { get; set; } // Khóa chính
        public string Name { get; set; } // Tên khách hàng
        public bool? Gender { get; set; } // Giới tính
        public string PhoneNumber { get; set; } // Số điện thoại
        public string? Password { get; set; }
        public string? Address { get; set; }
    }
}
