﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Entities
{
	public class User: IdentityUser<Guid>
	{
        public string? FullName { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
		public virtual ICollection<BookingDetail> BookingDetails { get; set; }
		public virtual ICollection<ShiftHandover> ShiftHandovers { get; set;}
		public virtual ICollection<EmployeeSchedule> EmployeeSchedules { get; set;}
		public virtual ICollection<EmployeeAttendance> EmployeeAttendances { get; set;}
		public virtual ICollection<EmployeeService> EmployeeServices { get; set;}
	}
}
