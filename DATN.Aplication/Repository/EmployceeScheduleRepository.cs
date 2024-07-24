using DATN.Aplication.Common;
using DATN.Aplication.Repository.IRepository;
using DATN.Data.EF;
using DATN.Data.Entities;

namespace DATN.Aplication.Repository
{
    public class EmployceeScheduleRepository : GenericRepository<EmployeeSchedule>, IEmployeeScheduleRepository
    {
        public EmployceeScheduleRepository(DATNDbContext context) : base(context)
        {
        }
    }
}
