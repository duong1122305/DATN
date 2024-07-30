using DATN.Aplication.Common;
using DATN.Aplication.Repository.IRepository;
using DATN.Data.EF;
using DATN.Data.Entities;

namespace DATN.Aplication.Repository
{
    public class ComboServiceRepository : GenericRepository<ComboService>, IComboServiceRepository
    {
        public ComboServiceRepository(DATNDbContext context) : base(context)
        {
        }
    }
}
