using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Attendace
{
	public class ShiftVM
	{
        public int ID { get; set; }
        public string Name { get; set; }
        public TimeSpan? Start { get; set; }
        public TimeSpan? End { get; set; }
        public bool IsCheckin { get; set; }
        public bool? IsLate { get; set; }
    }
}
