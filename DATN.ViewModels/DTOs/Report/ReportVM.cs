using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Report
{
	public class ReportVM
	{
        public int ID { get; set; }
        public string NameCustomer { get; set; }
        public string DateRate { get; set; }
        public string Comment { get; set; }
        public int BillID { get; set; }
		public int Rate { get; set; }
	}
}
