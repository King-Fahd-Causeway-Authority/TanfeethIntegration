namespace TanfeethIntegration.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PastDateAttribute : ValidationAttribute
    {
        public PastDateAttribute() : base("التاريخ يجب أن يكون في الماضي.")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                // Consider a null value as not having a date set, thus it passes validation.
                // If a date is required, it should be marked with [Required].
                return ValidationResult.Success;
            }

            if (value is DateTime dateTimeValue)
            {
                if (dateTimeValue.Date < DateTime.UtcNow.Date)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    // You can use the ErrorMessageString if you want to use a custom message defined on the attribute declaration
                    return new ValidationResult(ErrorMessageString ?? "التاريخ يجب أن يكون في الماضي.");
                }
            }
            else
            {
                return new ValidationResult("صيغة التاريخ غير صحيحة.");
            }
        }
    }
}
