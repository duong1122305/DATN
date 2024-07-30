using DATN.Aplication.Common;
using DATN.Aplication.Repository.IRepository;
using DATN.Data.EF;
using DATN.Data.Entities;

namespace DATN.Aplication.Repository
{
    public class EmployeAttendanceRepository : GenericRepository<EmployeeAttendance>, IEmployAttendanceRepository
    {
        public EmployeAttendanceRepository(DATNDbContext context) : base(context)
        {
        }

        public async Task RemoveAttendance(EmployeeAttendance employeeAttendance)
        {

            _context.EmployeeAttendances.Remove(employeeAttendance);

        }
    }
}
