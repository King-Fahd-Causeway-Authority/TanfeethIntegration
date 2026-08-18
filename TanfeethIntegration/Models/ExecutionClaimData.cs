namespace TanfeethIntegration.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static TanfeethIntegration.Models.DateWithinTenYearsAttribute;

    public class ExecutionClaimData
    {
        [Required(ErrorMessage = "تاريخ المطالبة بالأداء مطلوب.")]
        [DataType(DataType.Date)]
        [DateNotInTheFuture(ErrorMessage = "يجب أن يكون تاريخ المطالبة بالأداء في الماضي وليس تاريخ اليوم أو تاريخ مستقبلي.")]
        //[DateWithinTenYears("ExecutionDocumentDate", ErrorMessage = "يجب ألا يتجاوز تاريخ المطالبة بالأداء تاريخ السند التنفيذي بأكثر من عشر سنوات.")]
        public DateTime ExecutionClaimDate { get; set; }

        [Required(ErrorMessage = "معرف نتيجة مطالبة التنفيذ مطلوب.")]
        public int ExecutionClaimResultId { get; set; }
        public IEnumerable<SelectListItem> ExecutionClaimRes { get; set; }


        [StringLength(50, ErrorMessage = "يجب ألا يتجاوز رقم الطلب 50 حرفًا.")]
        public string? ExecutionClaimRequestNumber { get; set; }

        [Required(ErrorMessage = "ملف مطالبة التنفيذ مطلوب.")]
        [FileValidation(".pdf", 2, ErrorMessage = "يجب أن يكون الملف بصيغة PDF ولا يتجاوز حجمه 2 ميغابايت.")]
        public IFormFile ExecutionClaimFile { get; set; }
        public string ExecutionClaimFileBase64 { get; set; }


        [Required(ErrorMessage = "ملف إثبات التقديم مطلوب.")]
        [FileValidation(".pdf", 2, ErrorMessage = "يجب أن يكون الملف بصيغة PDF ولا يتجاوز حجمه 2 ميغابايت.")]
        public IFormFile ProofOfSubmitFile { get; set; }
        public string ProofOfSubmitFileBase64 { get; set; }

        // Assume this property exists and is set correctly in your system
        public DateTime ExecutionDocumentDate { get; set; }
    }
    // Custom validation attribute to check that a file is in the allowed format and does not exceed a maximum size.

    // Custom validation attribute to ensure the date is in the past and not today's date.



    public class DateNotInTheFutureAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Check if value is of DateTime type
            if (value is DateTime)
            {
                var date = (DateTime)value;

                // Check if the date is in the future
                if (date.Date >= DateTime.Now.Date)
                {
                    // If it is, return a validation error
                    return new ValidationResult("The date cannot be in the future.");
                }
            }

            // If the value is not a date at all (which means this attribute has been used incorrectly),
            // or if the date is in the past (a valid scenario), return success.
            // However, typically, non-DateTime values should not trigger this validation.
            // Only DateTime values should be checked, hence why non-DateTime values are considered a pass.
            return ValidationResult.Success;
        }
    }
    public class DateWithinTenYearsAttribute : ValidationAttribute
    {
        private readonly string _referenceProperty;

        public DateWithinTenYearsAttribute(string referenceProperty)
        {
            _referenceProperty = referenceProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Get the referenced property and its value
            var referencePropertyInfo = validationContext.ObjectType.GetProperty(_referenceProperty);
            if (referencePropertyInfo == null)
            {
                return new ValidationResult($"Unknown property: {_referenceProperty}");
            }

            var referencePropertyValue = referencePropertyInfo.GetValue(validationContext.ObjectInstance, null);
            DateTime referenceDate;
            try
            {
                referenceDate = Convert.ToDateTime(referencePropertyValue);
            }
            catch (InvalidCastException)
            {
                return new ValidationResult($"The {_referenceProperty} is not a valid datetime");
            }

            // Perform validation logic
            if (value is DateTime dateToValidate)
            {
                if (dateToValidate > referenceDate.AddYears(10) || dateToValidate < referenceDate.AddYears(-10))
                {
                    return new ValidationResult($"The date must be within ten years of {_referenceProperty}.");
                }

                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid date format");
        }

        public class FileValidationAttribute : ValidationAttribute
        {
            private readonly string _allowedFormat;
            private readonly int _maxFileSizeInMB;

            public FileValidationAttribute(string allowedFormat, int maxFileSizeInMB)
            {
                _allowedFormat = allowedFormat;
                _maxFileSizeInMB = maxFileSizeInMB;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                // Assume 'value' is of type IFormFile as it's common for file uploads in ASP.NET Core
                var file = value as IFormFile;
                if (file != null)
                {
                    // Check the file format
                    if (!_allowedFormat.Equals(Path.GetExtension(file.FileName), StringComparison.OrdinalIgnoreCase))
                    {
                        return new ValidationResult($"The file must be a {_allowedFormat} file.");
                    }

                    // Check the file size
                    if (file.Length > _maxFileSizeInMB * 1024 * 1024)
                    {
                        return new ValidationResult($"The file must not exceed {_maxFileSizeInMB} MB.");
                    }

                    return ValidationResult.Success;
                }

                return new ValidationResult("No file uploaded.");
            }
        }
    }

}

