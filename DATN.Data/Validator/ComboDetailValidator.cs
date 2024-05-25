using DATN.Data.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Validator
{
    public class ComboDetailValidator:AbstractValidator<ComboDetail>
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
