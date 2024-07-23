using DATN.Data.Entities;
using FluentValidation;

namespace DATN.Data.Validator
{
    public class GuestValidator : AbstractValidator<Guest>
    {
        public GuestValidator()
        {
            RuleFor(c => c.PhoneNumber)
                .NotEmpty()
                .WithMessage("Vui lòng nhập số điện thoại để chúng tôi có thể liên hệ bạn khi cần thiết!!");

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("Vui lòng nhập tên người đặt dịch vụ!");

            RuleFor(c => c.Address)
                .NotEmpty()
                .WithMessage("Vui lòng điền địa chỉ của bạn!!");

            RuleFor(c => c.Name)
                .Matches(@"(^([a-zỳọáầảấờễàạằệếýộậốũứĩõúữịỗìềểẩớặòùồợãụủíỹắẫựỉỏừỷởóéửỵẳẹèẽổẵẻỡơôưăêâđ'A-ZỲỌÁẦẢẤỜỄÀẠẰỆẾÝỘẬỐŨỨĨÕÚỮỊỖÌỀỂẨỚẶÒÙỒỢÃỤỦÍỸẮẪỰỈỎỪỶỞÓÉỬỴẲẸÈẼỔẴẺỠƠÔƯĂÊÂĐ]{1,})((\s)([a-zỳọáầảấờễàạằệếýộậốũứĩõúữịỗìềểẩớặòùồợãụủíỹắẫựỉỏừỷởóéửỵẳẹèẽổẵẻỡơôưăêâđ'A-ZỲỌÁẦẢẤỜỄÀẠẰỆẾÝỘẬỐŨỨĨÕÚỮỊỖÌỀỂẨỚẶÒÙỒỢÃỤỦÍỸẮẪỰỈỎỪỶỞÓÉỬỴẲẸÈẼỔẴẺỠƠÔƯĂÊÂĐ]{1,}))*)$")
                .WithMessage("Tên người dùng đặt chưa đúng định dạng!!");

            RuleFor(c => c.PhoneNumber)
                .Matches(@"^(0|\+84)(3|7|8|9)(([0-9]){8,8})$")
                .WithMessage("Số điện thoại khách hàng chưa đúng định dạng!!");
        }
    }
}
