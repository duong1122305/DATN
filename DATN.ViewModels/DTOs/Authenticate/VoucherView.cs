using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Authenticate
{
    public class VoucherView
    {
        public int? Id { get; set; }
        public string VoucherCode { get; set; }

        public int Quantity { get; set; }

        public string VoucherName { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal DiscountPercent { get; set; }

        public double MaxMoneyDiscount { get; set; }

        public double MinMoneyApplicable { get; set; }
    }
}
