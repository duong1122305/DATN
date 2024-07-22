using DATN.Aplication.Common;
using DATN.Aplication.Repository.IRepository;
using DATN.Data.EF;
using DATN.Data.Entities;

namespace DATN.Aplication.Repository
{
    public class PetRepository : GenericRepository<Pet>, IPetRepository
    {
        public PetRepository(DATNDbContext context) : base(context)
        {
        }
    }
}
