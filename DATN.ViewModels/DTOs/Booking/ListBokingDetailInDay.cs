﻿using DATN.ViewModels.Enum;

namespace DATN.ViewModels.DTOs.Booking
{
    public class ListBokingDetailInDay
    {
        public string NameStaffService { get; set; }
        public int? IdBookingDetail { get; set; }
        public double Price { get; set; }
        public string PetName { get; set; }
        public string ServiceDetaiName { get; set; }
        public BookingDetailStatus Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime BookingTime { get; set; }

    }
}
