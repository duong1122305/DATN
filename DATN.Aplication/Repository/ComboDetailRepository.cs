using DATN.Aplication.Common;
using DATN.Aplication.Repository.IRepository;
using DATN.Data.EF;
using DATN.Data.Entities;

namespace DATN.Aplication.Repository
{
    public class ComboDetailRepository : GenericRepository<ComboDetail>, IComboDetailRepository
    {
        public ComboDetailRepository(DATNDbContext context) : base(context)
        {
        }
    }
}
