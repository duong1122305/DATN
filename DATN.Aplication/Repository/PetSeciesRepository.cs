using DATN.Aplication.Common;
using DATN.Aplication.Repository.IRepository;
using DATN.Data.EF;
using DATN.Data.Entities;

namespace DATN.Aplication.Repository
{
    public class PetSeciesRepository : GenericRepository<PetSpecies>, IPetSceciesRepository
    {
        public PetSeciesRepository(DATNDbContext context) : base(context)
        {
        }
    }
}
