using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace installments_Payment.Core.Extensions
{
    public class RequiredIfNotNullOrEmptyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult(ErrorMessage ?? "مقدار الزامی است");

            if (value is string str && string.IsNullOrWhiteSpace(str))
                return new ValidationResult(ErrorMessage ?? "مقدار الزامی است");

            return ValidationResult.Success;
        }
    }
}
