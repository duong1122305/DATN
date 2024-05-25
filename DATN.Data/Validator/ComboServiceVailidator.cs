using DATN.Data.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Validator
{
    public class ComboServiceVailidator : AbstractValidator<ComboService>
    {
        public ComboServiceVailidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("Tên combo không được để trống bé ơi!!");
            
            RuleFor(c => c.Description)
                .NotEmpty()
                .WithMessage("Ghi bừa đi!");

            RuleFor(c => c.Price)
                .NotEmpty()
                .WithMessage("Giá không điền định làm free à");
        }
    }
}
