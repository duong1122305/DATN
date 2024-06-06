using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.Aplication.Services.IServices;

namespace DATN.Aplication.Services
{
    public class ShiftManagementService : IShiftManagementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShiftManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
