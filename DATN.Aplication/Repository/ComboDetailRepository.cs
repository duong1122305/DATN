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
    public class ComboDetailRepository : GennericRepository<ComboDetail>, IComboDetailRepository
    {
        public ComboDetailRepository(DATNDbContext context) : base(context)
        {
        }
    }
}
