using DATN.Data.Config;
using DATN.Data.Entities;
using DATN.ViewModels.DTOs.Booking;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
            builder.ApplyConfiguration(new ImagePetConfig());
            builder.ApplyConfiguration(new CategoryProductConfig());
            builder.ApplyConfiguration(new ImageProductConfig());
            builder.ApplyConfiguration(new OrderDetailConfig());
            builder.ApplyConfiguration(new ProductConfig());
            builder.ApplyConfiguration(new ProductDetailConfig());
            builder.Entity<BookingView>().HasNoKey();
        }

        public DbSet<ActionBooking> ActionBookings { get; set; }
        public DbSet<BookingView> BookingViews { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingDetail> bookingDetails { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<EmployeeAttendance> EmployeeAttendances { get; set; }
        public DbSet<EmployeeSchedule> EmployeeSchedules { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<HistoryAction> HistoryActions { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<PetSpecies> PetSpecies { get; set; }
        public DbSet<PetType> PetTypes { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceDetail> ServiceDetails { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<TypePayment> TypePayments { get; set; }
        public DbSet<WorkShift> WorkShifts { get; set; }
        public DbSet<ImagePet> ImagePets { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryDetails> CategoryDetails { get; set; }
        public DbSet<ImageProduct> ImageProducts { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
    }
}
