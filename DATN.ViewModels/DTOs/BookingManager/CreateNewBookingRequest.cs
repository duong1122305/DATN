using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.BookingManager
{
    public class CreateNewBookingRequest
    {
        public Guid IdStaff { get; set; }
        public Guid IdGuest { get; set; }
        public List<int> ListService { get; set; }
        public int MyProperty { get; set; }
    }
}
