﻿using DATN.ViewModels.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Booking
{
    public class BookingDetailView
    {
        public Guid? StaffId { get; set; }
        public string? ServiceDetailName { get; set; }
        public string? StaffName { get; set; }
        public int ServiceDetailId { get; set; }
        public TimeSpan StartDateTime { get; set; }

        public TimeSpan EndDateTime { get; set; }

        public DateTime DateBooking { get; set; }

        public BookingDetailStatus Status { get; set; }

        public double Price { get; set; }
    }
}
