using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Statistical
{
	public class ProductOutStock
	{
        public string FullName { get; set; }
		public string Category {  get; set; }
		public string Brand { get; set; }
        public bool Status { get; set; }
        public string Quantity { get; set; }
	}
}
