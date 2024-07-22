using DATN.Aplication.Common;
using DATN.Aplication.Repository.IRepository;
using DATN.Data.EF;
using DATN.Data.Entities;

namespace DATN.Aplication.Repository
{
    public class PetTypeRepository : GenericRepository<PetType>, IPetTypeRepository
    {
        public PetTypeRepository(DATNDbContext context) : base(context)
        {
        }
    }
}
