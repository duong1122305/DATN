using DATN.Aplication.Common;
using DATN.Aplication.Repository.IRepository;
using DATN.Data.EF;
using DATN.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Repository
{
    public class PetSeciesRepository : GennericRepository<PetSpecies>, IPetSceciesRepository
    {
        public PetSeciesRepository(DATNDbContext context) : base(context)
        {
        }
    }
}
