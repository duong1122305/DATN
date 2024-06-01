using DATN.Aplication.Common;
using DATN.Aplication.Repository;
using DATN.Data.EF;
using DATN.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DATNDbContext _context;

        public UnitOfWork(DATNDbContext context)
        {
            _context = context;
        }

        private IGenericRepository<Service> _ServiceRepository;

        private IGenericRepository<Booking> _BookingRepository;

        private IGenericRepository<BookingDetail> _BookingDetailRepository;

        private IGenericRepository<ComboDetail> _ComboDetailRepository;

        private IGenericRepository<ComboService> _ComboServiceRepository;

        private IGenericRepository<Discount> _DiscountRepository;

        private IGenericRepository<EmployeeSchedule> _EmployeeScheduleRepository;

        private IGenericRepository<EmployeeAttendance> _EmployeeAttendanceRepository;

        private IGenericRepository<Guest> _GuestRepository;

        private IGenericRepository<Pet> _PetRepository;

        private IGenericRepository<PetSpecies> _PetSpeciesRepository;

        private IGenericRepository<PetType> _PetTypeRepository;

        private IGenericRepository<Report> _ReportRepository;

        private IGenericRepository<ServiceDetail> _ServiceDetailRepository;

        private IGenericRepository<Shift> _ShiftRepository;

        private IGenericRepository<TypePayment> _TypePaymentRepository;

        private IGenericRepository<WorkShift> _WorkShiftRepository;
        public IGenericRepository<Service> ServiceRepository
        {
            get
            {
                if (_ServiceRepository == null)
                {
                    _ServiceRepository = new ServiceRepository(_context);
                }

                return _ServiceRepository;
            }
        }
        public IGenericRepository<Booking> BookingRepository
        {
            get
            {
                if (_BookingRepository == null)
                {
                    _BookingRepository = new BookingRepository(_context);
                }
                return _BookingRepository;
            }
        }

        public IGenericRepository<BookingDetail> BookingDetailRepository
        {
            get
            {
                if (_BookingDetailRepository == null)
                {
                    _BookingDetailRepository = new BookingDetailRepository(_context);
                }
                return _BookingDetailRepository;
            }
        }

        public IGenericRepository<ComboDetail> ComboDetailRepository
        {
            get
            {
                if (_ComboDetailRepository == null)
                {
                    _ComboDetailRepository = new ComboDetailRepository(_context);
                }
                return _ComboDetailRepository;
            }
        }

        public IGenericRepository<ComboService> ComboServiceRepository
        {
            get
            {
                if (_ComboServiceRepository == null)
                {
                    _ComboServiceRepository = new ComboServiceRepository(_context);
                }
                return _ComboServiceRepository;
            }
        }

        public IGenericRepository<Discount> DiscountRepository
        {
            get
            {
                if (_DiscountRepository == null)
                {
                    _DiscountRepository = new DiscountRepository(_context);
                }
                return _DiscountRepository;
            }
        }

        public IGenericRepository<EmployeeSchedule> EmployeeScheduleRepository
        {
            get
            {
                if (_EmployeeScheduleRepository == null)
                {
                    _EmployeeScheduleRepository = new EmployceeScheduleRepository(_context);
                }
                return _EmployeeScheduleRepository;
            }
        }

        public IGenericRepository<EmployeeAttendance> EmployeeAttendanceRepository
        {
            get
            {
                if (_EmployeeAttendanceRepository == null)
                {
                    _EmployeeAttendanceRepository = new EmployeAttendanceRepository(_context);
                }
                return _EmployeeAttendanceRepository;
            }
        }

        public IGenericRepository<Guest> GuestRepository
        {
            get
            {
                if (_GuestRepository == null)
                {
                    _GuestRepository = new GuestRepository(_context);
                }
                return _GuestRepository;
            }
        }

        public IGenericRepository<Pet> PetRepository
        {
            get
            {
                if (_PetRepository == null)
                {
                    _PetRepository = new PetRepository(_context);
                }
                return _PetRepository;
            }
        }

        public IGenericRepository<PetSpecies> PetSpeciesRepository
        {
            get
            {
                if (_PetSpeciesRepository == null)
                {
                    _PetSpeciesRepository = new PetSeciesRepository(_context);
                }
                return _PetSpeciesRepository;
            }
        }

        public IGenericRepository<Report> ReportRepository
        {
            get
            {
                if (_ReportRepository == null)
                {
                    _ReportRepository = new ReportRepository(_context);
                }
                return _ReportRepository;
            }
        }

        public IGenericRepository<PetType> PetTypeRepository
        {
            get
            {
                if (_PetTypeRepository == null)
                {
                    _PetTypeRepository = new PetTypeRepository(_context);
                }
                return _PetTypeRepository;
            }
        }

        public IGenericRepository<ServiceDetail> ServiceDetailRepository
        {
            get
            {
                if (_ServiceDetailRepository == null)
                {
                    _ServiceDetailRepository = new ServiceDetailRepository(_context);
                }
                return _ServiceDetailRepository;
            }
        }

        public IGenericRepository<Shift> ShiftRepository
        {
            get
            {
                if (_ShiftRepository == null)
                {
                    _ShiftRepository = new ShiftRepository(_context);
                }
                return _ShiftRepository;
            }
        }

        public IGenericRepository<TypePayment> TypePaymentRepository
        {
            get
            {
                if (_TypePaymentRepository == null)
                {
                    _TypePaymentRepository = new TypePaymentRepository(_context);
                }
                return _TypePaymentRepository;
            }
        }

        public IGenericRepository<WorkShift> WorkShiftRepository
        {
            get
            {
                if (_WorkShiftRepository == null)
                {
                    _WorkShiftRepository = new WorkShiftRepository(_context);
                }
                return _WorkShiftRepository;
            }
        }

        public async Task<int> SaveChangeAsync()
        {
           return await  _context.SaveChangesAsync();
        }
    }
}
