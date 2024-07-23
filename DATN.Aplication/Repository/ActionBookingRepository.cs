using DATN.Aplication.Common;
using DATN.Aplication.Repository.IRepository;
using DATN.Data.EF;
using DATN.Data.Entities;

namespace DATN.Aplication.Repository
{
    public class ActionBookingRepository : GenericRepository<ActionBooking>, IActionBookingRepository
    {
        public ActionBookingRepository(DATNDbContext context) : base(context)
        {
        }
    }
}
