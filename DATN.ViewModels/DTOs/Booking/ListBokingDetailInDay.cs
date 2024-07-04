using DATN.ViewModels.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Booking
{
    public class ListBokingDetailInDay
    {
        public string NameStaffService { get; set; }
        public double Price { get; set; }
        public string PetName { get; set; }
        public string ServiceDetaiName { get; set; }
        public BookingDetailStatus Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime BookingTime { get; set; }

    }
}
