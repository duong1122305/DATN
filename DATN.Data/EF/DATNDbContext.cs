using DATN.Data.Config;
using DATN.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.EF
{
    public class DATNDbContext : IdentityDbContext<User, Role, Guid>
    {
        public DATNDbContext(DbContextOptions<DATNDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new BookingConfig());
            builder.ApplyConfiguration(new BookingDetailConfig());
            builder.ApplyConfiguration(new ComboDetailConfig());
            builder.ApplyConfiguration(new ComboServiceConfig());
            builder.ApplyConfiguration(new DiscountConfig());
            builder.ApplyConfiguration(new EmployeeAttendanceConfig());
            builder.ApplyConfiguration(new EmployeeScheduleConfig());
            builder.ApplyConfiguration(new GuestConfig());
            builder.ApplyConfiguration(new PetConfig());
            builder.ApplyConfiguration(new PetSeciesConfig());
            builder.ApplyConfiguration(new PetTypeConfig());
            builder.ApplyConfiguration(new ReportConfig());
            builder.ApplyConfiguration(new ServiceConfig());
            builder.ApplyConfiguration(new ServiceDetailConfig());
            builder.ApplyConfiguration(new ShiftConfig());
            builder.ApplyConfiguration(new TypePaymentConfig());
            builder.ApplyConfiguration(new WorkShiftConfig());
        }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingDetail> bookingDetails { get; set; }
        public DbSet<ComboDetail> comboDetails { get; set; }
        public DbSet<ComboService> comboServices { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<EmployeeAttendance> EmployeeAttendances { get; set; }
        public DbSet<EmployeeSchedule> EmployeeSchedules { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<PetSpecies> PetSpecies { get; set; }
        public DbSet<PetType> PetTypes { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceDetail> ServiceDetails { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<TypePayment> TypePayments { get; set; }
        public DbSet<WorkShift> WorkShifts { get; set; }
    }
}
