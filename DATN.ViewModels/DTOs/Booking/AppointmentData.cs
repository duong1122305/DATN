﻿using DATN.Data.Enum;

namespace DATN.ViewModels.DTOs.Booking
{
    public class AppointmentData
    {
        public int Id { get; set; }
        public string Subject { get; set; }//tên khách hàng
        public string Description { get; set; }// tên dịch bụ
        public int BookingId { get; set; }//
        public int BookingServiceID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public BookingStatus Status { get; set; }
        public string? StatusName { get; set; }
    }
}
