using DATN.Aplication.Common;
using DATN.Data.Entities;

namespace DATN.Aplication.Repository.IRepository
{
    public interface IReportRepository : IGenericRepository<Report>
    {
		 Task<bool> Remove(Report rp);

	}
}