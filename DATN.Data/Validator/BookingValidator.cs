using DATN.Data.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Validator
{
    public class BookingValidator : AbstractValidator<Booking>
    {
        public BookingValidator()
        {
            RuleFor(c => c.GuestId)
                .Empty()
                .WithMessage("Không xác định được người đặt dịch vụ");

            RuleFor(c => c.TotalPrice)
                .NotEmpty()
                .WithMessage("Tổng tiền dịch vụ trống!!");
            
            RuleFor(c => c.BookingTime)
                .NotEmpty()
                .WithMessage("Thời gian đặt đơn chưa có!!");
            
            RuleFor(c => c.Status)
                .NotEmpty()
                .WithMessage("Trạng thái đơn đặt hàng chưa có!!");
        }
    }
}
