﻿using DATN.ViewModels.Enum;
using System.ComponentModel.DataAnnotations;

namespace DATN.ViewModels.DTOs.Authenticate
{
    public class VoucherView
    {
        public int? Id { get; set; }
        public string? VoucherCode { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập vào trường này")]
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
        [Range(1, 100, ErrorMessage = "Phải nằm trong khoảng 1-100%")]
        [Required(ErrorMessage = "Vui lòng nhập vào trường này")]
        public decimal DiscountPercent { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập vào trường này")]
        [Range(1000, int.MaxValue, ErrorMessage = "Số tiền nhập vào phải lớn hơn 1000")]
        public double MaxMoneyDiscount { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập vào trường này")]
        [Range(0, int.MaxValue, ErrorMessage = "Số tiền nhập vào phải lớn hơn 0")]
        public double MinMoneyApplicable { get; set; }
        public int? AmountUsed { get; set; }
        public VoucherStatus Status { get; set; }

    }
}
