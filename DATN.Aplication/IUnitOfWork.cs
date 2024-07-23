using DATN.Aplication.Repository.IRepository;

namespace DATN.Aplication
{
    public interface IUnitOfWork
    {
        IServiceRepository ServiceRepository { get; }
        IActionBookingRepository ActionBookingRepository { get; }
        IHistoryActionRepository HistoryActionRepository { get; }
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
        IBrandRepository BrandRepository { get; }
        ICategoryDetailRepository CategoryDetailRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IImageProductRepository ImageProductRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }
        IProductRepository ProductRepository { get; }
        IProductDetailRepository ProductDetailRepository { get; }
        Task<int> SaveChangeAsync();
    }
}
