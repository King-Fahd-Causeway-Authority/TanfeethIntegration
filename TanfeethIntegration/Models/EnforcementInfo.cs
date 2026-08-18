namespace TanfeethIntegration.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class EnforcementInfo
    {
        [Required(ErrorMessage = "هذا الحقل مطلوب.")]
        public int EnforcementTypeId { get; set; }

        public SelectList ExecutionClaimRes { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب.")]
        [StringLength(50, ErrorMessage = "لا يجب أن يتجاوز رقم التنفيذ 50 حرفًا.")]
        [RegularExpression("^[A-Za-z0-9]+$", ErrorMessage = "يجب أن يكون رقم التنفيذ أبجديًا رقميًا بدون مسافات.")]
        public string EnforcementNumber { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب.")]
        [DataType(DataType.Date)]
        [PastDate(ErrorMessage = "يجب أن يكون تاريخ التنفيذ أقل من تاريخ اليوم.")]
        public DateTime EnforcementDate { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب.")]
        [StringLength(200, ErrorMessage = "لا يجب أن يتجاوز مكان صدور التنفيذ 200 حرفًا.")]
        public string EnforcementIssuePlace { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب.")]
        [StringLength(200, ErrorMessage = "لا يجب أن يتجاوز اسم جهة الإصدار 200 حرفًا.")]
        public string EnforcementIssuer { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب.")]
        public bool HasFinancialClaim { get; set; }

        [RequiredIf("HasFinancialClaim", true, "المبلغ مطلوب في حال وجود دعوى مالية.")]
        [DataType(DataType.Currency)]
        public decimal EnforcementAmount { get; set; }

        [RequiredIf("HasFinancialClaim", true,  "قيمة المبلغ مطلوبة بالحروف في حال وجود دعوى مالية.")]
        [StringLength(50, ErrorMessage = "يجب ألا يتجاوز المبلغ المدون بالحروف 50 حرفًا.")]
        public string EnforcementAmountLetters { get; set; }

        [RequiredIf("HasFinancialClaim", true, "رقم الآيبان للمدعي مطلوب في حال وجود دعوى مالية.")]
        [StringLength(24, MinimumLength = 24, ErrorMessage = "يجب أن يكون رقم الآيبان 24 حرفًا.")]
        [RegularExpression("^SA[0-9]{2}[A-Za-z0-9]+$", ErrorMessage = "يجب أن يبدأ الآيبان بـ 'SA' يتبعه 22 حرفًا أبجديًا رقميًا.")]
        public string PlaintiffIBAN { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب.")]
        [FileBase64(".pdf", 2, ErrorMessage = "يجب أن يكون ملف التنفيذ بصيغة PDF ولا يتجاوز حجمه 2 ميجابايت.")]
        public IFormFile EnforcementFile { get; set; }
        public string EnforcementFileBase64 { get; set; }


        [FileBase64(".pdf", 2, 3, ErrorMessage = "يجب أن تكون الملفات الإضافية بصيغة PDF ولا يتجاوز حجم كل واحد منها 2 ميجابايت والعدد الأقصى هو 3.")]
        public List<IFormFile>? EnforcementExtraFiles { get; set; }
        public List<string> EnforcementExtraFilesBase64 { get; set; }


        [Required(ErrorMessage = "هذا الحقل مطلوب.")]
        [StringLength(20000, ErrorMessage = "لا يجب أن يتجاوز بيان التنفيذ 20000 حرف.")]
        public string EnforcementStatement { get; set; }

        // List of execution items for the enforcement. Up to 30 items.
       
        public List<string> EnforcementItems { get; set; }
    }
    // Custom validation attributes


    public class RequiredIfAttribute : ValidationAttribute
    {
        private readonly string _propertyName;
        private readonly object _desiredValue;

        public RequiredIfAttribute(string propertyName, object desiredValue, string errorMessage)
            : base(errorMessage)
        {
            _propertyName = propertyName;
            _desiredValue = desiredValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyInfo = validationContext.ObjectType.GetProperty(_propertyName);
            if (propertyInfo == null)
            {
                return new ValidationResult($"Unknown property: {_propertyName}");
            }

            var propertyValue = propertyInfo.GetValue(validationContext.ObjectInstance, null);
            if (Equals(propertyValue, _desiredValue) && value == null)
            {
                return new ValidationResult(ErrorMessage ?? "هذا الحقل مطلوب عندما تكون القيمة المحددة معينة."); // Arabic text
            }


            return ValidationResult.Success;
        }
    }

    // Custom validation attribute to verify a string is a base64-encoded file with specific constraints
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class FileBase64Attribute : ValidationAttribute
    {
        public string AllowedExtensions { get; private set; }
        public int MaxFileSizeMB { get; private set; }
        public int MaxFileCount { get; private set; }

        public FileBase64Attribute(string allowedExtension, int maxFileSizeMB)
        {
            AllowedExtensions = allowedExtension;
            MaxFileSizeMB = maxFileSizeMB;
            MaxFileCount = 1; // Default value for max file count, assuming single file by default
        }

        public FileBase64Attribute(string allowedExtension, int maxFileSizeMB, int maxFileCount)
            : this(allowedExtension, maxFileSizeMB)
        {
            MaxFileCount = maxFileCount; // This allows setting the max file count through the attribute
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var files = value as List<string>;
            if (files == null)
            {
                // If value is not a list, this attribute doesn't apply.
                return ValidationResult.Success;
            }

            if (files.Count > MaxFileCount)
            {
                return new ValidationResult($"لا يمكن أن يتجاوز عدد الملفات {MaxFileCount}."); // Arabic text
            }

            foreach (var base64EncodedString in files)
            {
                // Check if the base64 string is empty
                if (string.IsNullOrWhiteSpace(base64EncodedString))
                {
                    return new ValidationResult("لا يمكن أن يكون الملف فارغًا."); // Arabic text
                }

                // Check for allowed file extensions (if the base64 string contains mime type)
                // and check for maximum file size:

                // Extract byte array for the file from the base64 string
                var indexOfComma = base64EncodedString.IndexOf(',');
                var base64String = indexOfComma >= 0 ? base64EncodedString.Substring(indexOfComma + 1) : base64EncodedString;
                byte[] fileBytes;
                try
                {
                    fileBytes = Convert.FromBase64String(base64String);
                }
                catch (FormatException)
                {
                    return new ValidationResult("The file is not a valid base64 encoded string.");
                }

                // Validate the file size
                if (fileBytes.Length > MaxFileSizeMB * 1024 * 1024)
                {
                    return new ValidationResult($"يجب ألا يتجاوز حجم كل ملف {MaxFileSizeMB} ميغابايت."); // Arabic text
                }

                // Here, you'd add logic to validate the file's extension if it's implemented

                // If all checks pass, continue checking the next file in the list
            }

            // If there are no invalid files, the overall validation succeeds
            return ValidationResult.Success;
        }
    }
}
