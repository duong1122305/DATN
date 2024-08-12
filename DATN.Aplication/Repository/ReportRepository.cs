using DATN.Aplication.Common;
using DATN.Aplication.Repository.IRepository;
using DATN.Data.EF;
using DATN.Data.Entities;

namespace DATN.Aplication.Repository
{
    public class ReportRepository : GenericRepository<Report>, IReportRepository
    {
        public ReportRepository(DATNDbContext context) : base(context)
        {
        }
        public async Task<bool> Remove(Report rp)
        {
            try
            {
                _context.Remove(rp);
                return await _context.SaveChangesAsync()>0;
                
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
