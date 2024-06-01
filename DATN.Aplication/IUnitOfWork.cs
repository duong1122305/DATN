using DATN.Aplication.Common;
using DATN.Aplication.Repository;
using DATN.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication
{
    public interface IUnitOfWork
    {
        IGenericRepository<Service> ServiceRepository { get; }
        IGenericRepository<Booking> BookingRepository { get; }
        IGenericRepository<BookingDetail> BookingDetailRepository { get; }
        IGenericRepository<ComboDetail> ComboDetailRepository { get; }
        IGenericRepository<ComboService> ComboServiceRepository { get; }
        IGenericRepository<Discount> DiscountRepository { get; }
        IGenericRepository<EmployeeSchedule> EmployeeScheduleRepository { get; }
        IGenericRepository<EmployeeAttendance> EmployeeAttendanceRepository { get; }
        IGenericRepository<Guest> GuestRepository { get; }
        IGenericRepository<Pet> PetRepository { get; }
        IGenericRepository<PetSpecies> PetSpeciesRepository { get; }
        IGenericRepository<PetType> PetTypeRepository { get; }
        IGenericRepository<Report> ReportRepository { get; }
        IGenericRepository<ServiceDetail> ServiceDetailRepository { get; }
        IGenericRepository<Shift> ShiftRepository { get; }
        IGenericRepository<TypePayment> TypePaymentRepository { get; }
        IGenericRepository<WorkShift> WorkShiftRepository { get; }
    }
}
