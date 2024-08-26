using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Booking
{
	public class BillPrintVM
	{
        public BillPrintVM()
        {
            DataPrintBills = new List<DataPrintBill>();
        }
        public  string CusName { get; set; }
        public  string StaffName { get; set; }
		public string  PhoneNumber { get; set; }
        public string TotalAmount { get; set; }
        public string TotalReduce { get; set; }
        public string AmountPayment { get; set; }
        public string TimePayment { get; set; }
        public string Address { get; set; }
        public string QrCheckOut { get; set; }
       public List<DataPrintBill> DataPrintBills { get; set; }

    }
}
