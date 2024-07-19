using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Attendace
{
	public class ListPerAttenMonth
	{
        public AttendanceMonth AllData { get; set; }
       public List<AttendancePerMonth> AttendancePerMonths { get; set;}= new List<AttendancePerMonth>();
    }
}
