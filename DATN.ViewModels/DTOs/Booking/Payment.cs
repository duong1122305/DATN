﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Booking
{
    public class Payment
    {
        public Guid IdGuest { get; set; }
        public string Token { get; set; }
        public DateTime DateBooking { get; set; }
        public int TypePaymenId { get; set; }
        public int VoucherId { get; set; }
        public double TotalPrice { get; set; }
        public double Reduce { get; set; }

    }
}