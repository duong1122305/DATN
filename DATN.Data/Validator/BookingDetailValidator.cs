using DATN.Data.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Validator
{
    public class BookingDetailValidator : AbstractValidator<BookingDetail>
    {
        public BookingDetailValidator()
        {
            RuleFor(c => c.BookingId)
                .NotEmpty()
                .WithMessage("Không biết của booking nào!");

            RuleFor(c => c.StaffId)
                .NotEmpty()
                .WithMessage("Chưa biết nhân viên nào làm dịch vụ này");

            RuleFor(c => c)
                .Must(c => c.ComboId != 0 && c.ServiceDetailId != 0)
                .WithMessage("Chưa chọn dịch vụ hoặc chọn combo");
        }
    }
}
