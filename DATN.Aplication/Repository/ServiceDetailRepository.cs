using DATN.Aplication.Common;
using DATN.Aplication.Repository.IRepository;
using DATN.Data.EF;
using DATN.Data.Entities;

namespace DATN.Aplication.Repository
{
    public class ServiceDetailRepository : GenericRepository<ServiceDetail>, IServiceDetailsRepository
    {
        public ServiceDetailRepository(DATNDbContext context) : base(context)
        {
        }
    }
}
