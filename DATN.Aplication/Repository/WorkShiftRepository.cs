using DATN.Aplication.Common;
using DATN.Aplication.Repository.IRepository;
using DATN.Data.EF;
using DATN.Data.Entities;

namespace DATN.Aplication.Repository
{
    public class WorkShiftRepository : GenericRepository<WorkShift>, IWorkShiftRepository
    {
        public WorkShiftRepository(DATNDbContext context) : base(context)
        {
        }
    }
}
