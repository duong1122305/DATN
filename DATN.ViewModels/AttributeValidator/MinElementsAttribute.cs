using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.AttributeValidator
{
	public class MinElementsAttribute : ValidationAttribute
	{
		private readonly int _minElements;

		public MinElementsAttribute(int minElements)
		{
			_minElements = minElements;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value is IList list && list.Count >= _minElements)
			{
				return ValidationResult.Success;
			}

			return new ValidationResult(ErrorMessage ?? $"Phải có ít nhất {_minElements} biến thể.");
		}
	}
}
