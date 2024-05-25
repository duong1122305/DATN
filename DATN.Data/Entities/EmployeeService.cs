using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Entities
{
    public class EmployeeService
    {
        public int  IdService { get; set; }
        public Guid IdCustomer { get; set; }
        public virtual User User { get; set; }
        public virtual Service Service { get; set; }
    }
}
