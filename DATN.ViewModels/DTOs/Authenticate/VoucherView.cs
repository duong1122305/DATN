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
        [Required(ErrorMessage = "Vui lòng nhập vào trường này")]
        [RegularExpression("^([a-zA-Z]){15,15}$",ErrorMessage ="Mã giảm giá phải đủ 15 ký tự và viết ko dấu")]
        [MaxLength(15)]
        public string VoucherCode { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập vào trường này")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập vào trường này")]
        public string VoucherName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập vào trường này")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập vào trường này")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập vào trường này")]
        public DateTime EndDate { get; set; }
        [Range(1,100,ErrorMessage ="Phải nằm trong khoảng 1-100%")]
        [Required(ErrorMessage = "Vui lòng nhập vào trường này")]
        public decimal DiscountPercent { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập vào trường này")]
        [Range(1000, int.MaxValue, ErrorMessage = "Số tiền nhập vào phải lớn hơn 1000")]
        public double MaxMoneyDiscount { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập vào trường này")]
        [Range(0, int.MaxValue, ErrorMessage = "Số tiền nhập vào phải lớn hơn 0")]
        public double MinMoneyApplicable { get; set; }

    }
}
