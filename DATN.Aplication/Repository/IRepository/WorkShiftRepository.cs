using DATN.Aplication.Common;
using DATN.Data.EF;
using DATN.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Repository.IRepository
{
    public class WorkShiftRepository : GennericRepository<WorkShift>, IWorkShiftRepository
    {
        public WorkShiftRepository(DATNDbContext context) : base(context)
        {
        }
    }
}
