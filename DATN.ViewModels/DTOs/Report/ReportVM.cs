using System;
using System.Collections.Generic;
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
        public string NameStaff { get; set; }
        public string ServiceName {  get; set; }
        public Guid StaffID { get; set; }
        public int BillID { get; set; }
    }
}
