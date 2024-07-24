using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DATN.Utilites.Validator
{
    public class PasswordValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var password = value as string;

            if (string.IsNullOrEmpty(password) || password.Length <= 6)
            {
                return new ValidationResult("Mật khẩu phải có độ dài trên 6 ký tự.");
            }

            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                return new ValidationResult("Mật khẩu phải chứa ít nhất một chữ cái viết hoa.");
            }

            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                return new ValidationResult("Mật khẩu phải chứa ít nhất một chữ cái viết thường.");
            }

            if (!Regex.IsMatch(password, @"\d"))
            {
                return new ValidationResult("Mật khẩu phải chứa ít nhất một chữ số.");
            }

            if (!Regex.IsMatch(password, @"[\W_]"))
            {
                return new ValidationResult("Mật khẩu phải chứa ít nhất một ký tự đặc biệt.");
            }

            return ValidationResult.Success;
        }
    }
}
