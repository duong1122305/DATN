using DATN.Aplication.Common;
using DATN.Aplication.Repository;
using DATN.Aplication.Repository.IRepository;
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
        IServiceRepository ServiceRepository { get; }
        IBookingRepository BookingRepository { get; }
        IBookingDetailRepository BookingDetailRepository { get; }
        IComboDetailRepository ComboDetailRepository { get; }
        IComboServiceRepository ComboServiceRepository { get; }
        IDiscountRepository DiscountRepository { get; }
        IEmployeeScheduleRepository EmployeeScheduleRepository { get; }
        IEmployAttendanceRepository EmployeeAttendanceRepository { get; }
        IGuestRepository GuestRepository { get; }
        IPetRepository PetRepository { get; }
        IPetSceciesRepository PetSpeciesRepository { get; }
        IPetTypeRepository PetTypeRepository { get; }
        IReportRepository ReportRepository { get; }
        IServiceDetailsRepository ServiceDetailRepository { get; }
        IShiftRepository ShiftRepository { get; }
        ITypePaymentRepository TypePaymentRepository { get; }
        IWorkShiftRepository WorkShiftRepository { get; }
        Task<int> SaveChangeAsync();
    }
}
