using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TanfeethIntegration.Models
{
    public class LegalRepresentative
    {
        [Required(ErrorMessage ="هذا الحقل مطلوب")]
        public int AgencyId { get; set; }
        public SelectList AgencyOptions { get; set; }

        public long IdentityTypeId { get; set; }

        
        public long LegalRepIdentity { get; set; }

        [DataType(DataType.Date)]

        public DateTime DateOfBirth { get; set; }

        public string? Name { get; set; }

        
        public string? Nationality { get; set; }

        [Phone(ErrorMessage = "رقم الهاتف غير صالح.")]
        [RegularExpression(@"^\+966\d{9}$", ErrorMessage = "يجب أن يبدأ رقم الهاتف ب+966 وأن يكون متبوعًا بـ 9 أرقام.")]
        public string? MobileNumber { get; set; }

       
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح.")]
        public string? Email { get; set; }

        [Display(Name = "مرفق اثبات التمثيل")]
        [DataType(DataType.Upload)]
        [PdfFile(ErrorMessage = "فقط ملف PDF مسموح به.")]
        public IFormFile? DeedDocumentFile { get; set; }
        public string DeedDocumentFileBase64 { get; set; }


        // Nullable int for License Number. No data annotation is required if it's optional.
        public int? LicenseNumber { get; set; }

        // ... Other necessary properties and their validations ...
    }



    public class PdfFileAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            // If no file was uploaded (and assuming you don't have a [Required] attribute elsewhere), it's fine
            if (file == null)
            {
                return ValidationResult.Success;
            }

            if (Path.GetExtension(file.FileName).ToUpperInvariant() != ".PDF")
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
