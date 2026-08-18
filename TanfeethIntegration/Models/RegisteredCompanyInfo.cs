
namespace TanfeethIntegration.Models
{

    using System;
    using System.ComponentModel.DataAnnotations;

    public class RegisteredCompanyInfo
    {
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        // Nullable long allows us to enforce the presence when NationalUnifiedNumber is not provided
        [Range(1, long.MaxValue, ErrorMessage = "يجب أن يكون رقم السجل التجاري رقمًا موجبًا.")]
        public long CommercialRegistrationNumber { get; set; }

        [DataType(DataType.Date)]
        [PastDateOnly(ErrorMessage = "يجب أن يكون تاريخ بدء السجل التجاري في الماضي.")]
        public DateTime CrNumberStartDate { get; set; }

        [DataType(DataType.Date)]
        [GreaterThanOrEqualToOtherDate(nameof(CrNumberStartDate), ErrorMessage = "يجب أن يكون تاريخ انتهاء السجل التجاري لاحقًا لتاريخ البدء.")]
        public DateTime CrNumberEndDate { get; set; }

        [RegularExpression("7\\d{9}", ErrorMessage = "يجب أن يبدأ الرقم الوطني الموحد بـ7 وأن يكون طوله 10 أرقام.")]
        public long? NationalUnifiedNumber { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [StringLength(100, ErrorMessage = "لا يمكن أن يتجاوز طول اسم الشركة 100 حرف.")]
        public string CompanyName { get; set; }

        public string? IqamaNumber { get; set; }
        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression(@"^05\d{8,9}$", ErrorMessage = "رقم الهاتف غير صحيح")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب.")]
        [EmailAddress(ErrorMessage = "الرجاء إدخال بريد إلكتروني صالح.")]
        public string Email { get; set; }

        // The CompanyHeadquartersInfo class would similarly need its annotations translated if present.
        public CompanyHeadquartersInfo CompanyHeadquarters { get; set; }

        // Same for IndividualInfo class.
        public IndividualInfo ManagerInfo { get; set; }

    }
    // Custom validation for past date only
    public class PastDateOnlyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Cast the value to DateTime? since it could be nullable
            var date = value as DateTime?;

            // If the date has no value, or the date is not in the future, validation is successful
            if (!date.HasValue || date.Value.Date < DateTime.Now.Date)
            {
                return ValidationResult.Success;
            }

            // If the date is in the future, return a validation error
            // Here, use ErrorMessage to get the error message specified in the attribute
            // usage, or fall back to a default message.
            return new ValidationResult(ErrorMessage ?? "The date must not be in the future.");
        }
    }

    // Custom validation attribute to validate that one date is greater than or equal to another date
    public class GreaterThanOrEqualToOtherDateAttribute : ValidationAttribute
    {
        private readonly string _otherPropertyName;

        public GreaterThanOrEqualToOtherDateAttribute(string otherPropertyName)
        {
            _otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object firstValue, ValidationContext validationContext)
        {
            var firstDate = firstValue as DateTime?;

            var otherPropertyInfo = validationContext.ObjectType.GetProperty(_otherPropertyName);
            if (otherPropertyInfo == null)
                throw new ArgumentException("Property with this name not found");

            var secondDate = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null) as DateTime?;

            if (!firstDate.HasValue || !secondDate.HasValue)
                return ValidationResult.Success;  // Assume successful validation when dates aren't provided

            if (firstDate.Value.Date >= secondDate.Value.Date)
                return ValidationResult.Success;

            return new ValidationResult(ErrorMessage ?? $"This date must be greater than or equal to {_otherPropertyName}.");
        }
    }




}
