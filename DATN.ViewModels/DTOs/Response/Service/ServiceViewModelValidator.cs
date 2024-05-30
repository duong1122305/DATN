using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Response.Service
{
    public class ServiceViewModelValidator : AbstractValidator<ServiceViewModel>
    {
        public ServiceViewModelValidator()
        {

            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("Vui lòng nhập tên dịch vụ!");
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("Vui lòng nhập tên dịch vụ!");

        }

    }
}
