﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DATN.Data.Entities
{

    public class User : IdentityUser<Guid>
    {
        [Required]
        public string FullName { get; set; }
        public bool Gender { get; set; }
        public string? CodeConfirm { get; set; }
        public string Address { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DoB { get; set; } = DateTime.Now;
        public string? ImgUrl { get; set; }
        public string? ImgID { get; set; }
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
        public virtual ICollection<EmployeeSchedule> EmployeeSchedules { get; set; }
        public virtual ICollection<HistoryAction> HistoryActions { get; set; }
    }

}
