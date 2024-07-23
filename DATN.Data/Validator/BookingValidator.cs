using DATN.Data.Entities;
using FluentValidation;

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
