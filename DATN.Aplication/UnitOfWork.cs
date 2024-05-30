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
    public class UnitOfWork: IUnitOfWork
    {
        private readonly DATNDbContext _context;

        public UnitOfWork(DATNDbContext context)
        {
            _context = context;
        }
        private IGennericRepository<Service> serviceRepository;
        public IGennericRepository<Service> ServiceRepository
        {
            get
            {
                if (serviceRepository == null)
                {
                    serviceRepository = new ServiceRepository(_context);
                }

                return serviceRepository;
            }
        }
        public void SaveChangeAsync()
        {
            _context.SaveChangesAsync();
        }
    }
}
