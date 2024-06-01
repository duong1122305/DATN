using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Entities
{

    public class User : IdentityUser<Guid>
    {
        [Required]
        public string FullName { get; set; }
        public bool Gender { get; set; }
        public string? CodeConfirm { get; set; }
        public string Address { get; set; }
       
        public ICollection<Booking> BookingsAsCounterStaff { get; set; }
        public ICollection<Booking> BookingsAsConfirmStaff { get; set; }
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
        public virtual ICollection<EmployeeSchedule> EmployeeSchedules { get; set; }
        public virtual ICollection<EmployeeAttendance> EmployeeAttendances { get; set; }
    }

}
