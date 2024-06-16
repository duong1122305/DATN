using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Authenticate
{
    public class VoucherView
    {
        public int? Id { get; set; }
        [RegularExpression("^([a-zA-Z]){15,15}$",ErrorMessage ="Mã giảm giá phải đủ 15 ký tự và viết ko dấu")]
        public string VoucherCode { get; set; }

        public int Quantity { get; set; }

        public string VoucherName { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        [Range(1,100,ErrorMessage ="Phải nằm trong khoảng 1-100%")]
        public decimal DiscountPercent { get; set; }
        public double MaxMoneyDiscount { get; set; }
        public double MinMoneyApplicable { get; set; }
    }
}
