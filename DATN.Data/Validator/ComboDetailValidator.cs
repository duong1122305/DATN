using DATN.Data.Entities;
using FluentValidation;

namespace DATN.Data.Validator
{
    public class ComboDetailValidator : AbstractValidator<ComboDetail>
    {
        public ComboDetailValidator()
        {
            RuleFor(c => c.ServiceDetailId)
                .NotEmpty()
                .WithMessage("Không thể để trống trường này");

            RuleFor(c => c.ComboServiceId)
               .NotEmpty()
               .WithMessage("Không thể để trống trường này");
        }
    }
}
