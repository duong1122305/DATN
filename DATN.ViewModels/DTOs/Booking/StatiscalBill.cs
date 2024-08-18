using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Booking
{
	public class StatiscalBill
	{
        public int ID { get; set; }
        public  string TimeComplete { get; set; }
        public string CusName { get; set; }
        public string Amount { get; set; }
        public string AmountReal { get; set; }
        public string Status {  get; set; }
        public string StaffName { get; set; }
        public string AmountReduced { get; set; }
    }
}
