﻿using DATN.Aplication.Common;
using DATN.Aplication.Repository;
using DATN.Aplication.Repository.IRepository;
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
        private IServiceRepository _serviceRepository;
        private IBookingRepository _bookingRepository;
        private IBookingDetailRepository _bookingDetailRepository;
        private IComboDetailRepository _comboDetailRepository;
        private IComboServiceRepository _comboServiceRepository;
        private IDiscountRepository _discountRepository;
        private IEmployeeScheduleRepository _employeeScheduleRepository;
        private IEmployAttendanceRepository _employeeAttendanceRepository;
        private IGuestRepository _guestRepository;
        private IPetRepository _petRepository;
        private IPetSceciesRepository _petSpeciesRepository;
        private IPetTypeRepository _petTypeRepository;
        private IReportRepository _reportRepository;
        private IServiceDetailsRepository _serviceDetailRepository;
        private IShiftRepository _shiftRepository;
        private ITypePaymentRepository _typePaymentRepository;
        private IWorkShiftRepository _workShiftRepository;
        IBrandRepository _brandRepository;
        ICategoryProductRepository _categoryProductRepository;
        ICategoryRepository _categoryRepository;
        IImageProductRepository _imageProductRepository;
        IOrderDetailRepository _orderDetailRepository;
        IProductRepository _productRepository;
        IProductDetailRepository _productDetailRepository;

        public UnitOfWork(DATNDbContext context)
        {
            _context = context;
        }

        public IServiceRepository ServiceRepository
        {
            get
            {
                if (_serviceRepository == null)
                {
                    _serviceRepository = new ServiceRepository(_context);
                }
                return _serviceRepository;
            }
        }

        public IBookingRepository BookingRepository
        {
            get
            {
                if (_bookingRepository == null)
                {
                    _bookingRepository = new BookingRepository(_context);
                }
                return _bookingRepository;
            }
        }

        public IBookingDetailRepository BookingDetailRepository
        {
            get
            {
                if (_bookingDetailRepository == null)
                {
                    _bookingDetailRepository = new BookingDetailRepository(_context);
                }
                return _bookingDetailRepository;
            }
        }

        public IComboDetailRepository ComboDetailRepository
        {
            get
            {
                if (_comboDetailRepository == null)
                {
                    _comboDetailRepository = new ComboDetailRepository(_context);
                }
                return _comboDetailRepository;
            }
        }

        public IComboServiceRepository ComboServiceRepository
        {
            get
            {
                if (_comboServiceRepository == null)
                {
                    _comboServiceRepository = new ComboServiceRepository(_context);
                }
                return _comboServiceRepository;
            }
        }

        public IDiscountRepository DiscountRepository
        {
            get
            {
                if (_discountRepository == null)
                {
                    _discountRepository = new DiscountRepository(_context);
                }
                return _discountRepository;
            }
        }

        public IEmployeeScheduleRepository EmployeeScheduleRepository
        {
            get
            {
                if (_employeeScheduleRepository == null)
                {
                    _employeeScheduleRepository = new EmployceeScheduleRepository(_context);
                }
                return _employeeScheduleRepository;
            }
        }

        public IEmployAttendanceRepository EmployeeAttendanceRepository
        {
            get
            {
                if (_employeeAttendanceRepository == null)
                {
                    _employeeAttendanceRepository = new EmployeAttendanceRepository(_context);
                }
                return _employeeAttendanceRepository;
            }
        }

        public IGuestRepository GuestRepository
        {
            get
            {
                if (_guestRepository == null)
                {
                    _guestRepository = new GuestRepository(_context);
                }
                return _guestRepository;
            }
        }

        public IPetRepository PetRepository
        {
            get
            {
                if (_petRepository == null)
                {
                    _petRepository = new PetRepository(_context);
                }
                return _petRepository;
            }
        }

        public IPetSceciesRepository PetSpeciesRepository
        {
            get
            {
                if (_petSpeciesRepository == null)
                {
                    _petSpeciesRepository = new PetSeciesRepository(_context);
                }
                return _petSpeciesRepository;
            }
        }

        public IPetTypeRepository PetTypeRepository
        {
            get
            {
                if (_petTypeRepository == null)
                {
                    _petTypeRepository = new PetTypeRepository(_context);
                }
                return _petTypeRepository;
            }
        }

        public IReportRepository ReportRepository
        {
            get
            {
                if (_reportRepository == null)
                {
                    _reportRepository = new ReportRepository(_context);
                }
                return _reportRepository;
            }
        }

        public IServiceDetailsRepository ServiceDetailRepository
        {
            get
            {
                if (_serviceDetailRepository == null)
                {
                    _serviceDetailRepository = new ServiceDetailRepository(_context);
                }
                return _serviceDetailRepository;
            }
        }

        public IShiftRepository ShiftRepository
        {
            get
            {
                if (_shiftRepository == null)
                {
                    _shiftRepository = new ShiftRepository(_context);
                }
                return _shiftRepository;
            }
        }

        public ITypePaymentRepository TypePaymentRepository
        {
            get
            {
                if (_typePaymentRepository == null)
                {
                    _typePaymentRepository = new TypePaymentRepository(_context);
                }
                return _typePaymentRepository;
            }
        }

        public IWorkShiftRepository WorkShiftRepository
        {
            get
            {
                if (_workShiftRepository == null)
                {
                    _workShiftRepository = new WorkShiftRepository(_context);
                }
                return _workShiftRepository;
            }
        }

        public IBrandRepository BrandRepository
        {
            get
            {
                if (_brandRepository==null)
                {
                    _brandRepository = new BrandRepository(_context);
                }
                return _brandRepository;
            }
        }

        public ICategoryProductRepository CategoryProductRepository
        {
            get
            {
                if (_categoryProductRepository == null)
                {
                    _categoryProductRepository = new CategoryProductRepository(_context);
                }
                return _categoryProductRepository;
            }
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                if (_categoryRepository == null)
                {
                    _categoryRepository = new CategoryRepository(_context);
                }
                return _categoryRepository;
            }
        }

        public IImageProductRepository ImageProductRepository
        {
            get
            {
                if (_imageProductRepository == null)
                {
                    _imageProductRepository = new ImageProductRepository(_context);
                }
                return _imageProductRepository;
            }
        }
        public IOrderDetailRepository OrderDetailRepository
        {
            get
            {
                if (_orderDetailRepository == null)
                {
                    _orderDetailRepository = new OrderDetailRepository(_context);
                }
                return _orderDetailRepository;
            }
        }

        public IProductRepository ProductRepository
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository = new ProductRepository(_context);
                }
                return _productRepository;
            }
        }

        public IProductDetailRepository ProductDetailRepository
        {
            get
            {
                if (_productDetailRepository == null)
                {
                    _productDetailRepository = new ProductDetailRepository(_context);
                }
                return _productDetailRepository;
            }
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }

    
    }

}
