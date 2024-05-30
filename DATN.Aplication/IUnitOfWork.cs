using DATN.Aplication.Common;
using DATN.Aplication.Repository;
using DATN.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication
{
    public interface IUnitOfWork
    {
        IGennericRepository<Service> ServiceRepository { get; }
    }
}
