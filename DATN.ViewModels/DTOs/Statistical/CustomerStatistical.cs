using DATN.ViewModels.Common.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Statistical
{
	public class CustomerStatistical
	{
		public string Name { get; set; }
		public List<ChartData> Data { get; set; }

	}
}
