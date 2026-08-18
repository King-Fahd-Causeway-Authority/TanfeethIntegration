namespace TanfeethIntegration.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    public class EndowmentInfo
    {
        [Required]
        [StringLength(50, ErrorMessage = "Endowment registration number must not exceed 50 characters.")]
        [RegularExpression(@"^[A-Za-z0-9]*$", ErrorMessage = "Only English letters and numbers are accepted, with no spaces.")]
        public string EndowmentRegNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Endowment name must not exceed 100 characters.")]
        public string Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Deed number must not exceed 50 characters.")]
        [RegularExpression(@"^[A-Za-z0-9]*$", ErrorMessage = "Only English letters and numbers are accepted, with no spaces.")]
        public string DeedNumber { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression(@"^05\d{8,9}$", ErrorMessage = "رقم الهاتف غير صحيح")]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [PastDateOnly(ErrorMessage = "Deed registration date must be less than today's date.")]
        public DateTime DeedRegDate { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        public string Email { get; set; }

        // Assuming "بيانات ناظر الوقف" (Caretaker's Information) would be structured similarly to IndividualInfo
        public IndividualInfo CaretakerInfo { get; set; }

        // Assuming "بيانات مقر الوقف" (Endowment's Headquarters Information) includes address and similar fields
        public CompanyHeadquartersInfo HeadquartersInfo { get; set; }
    }

    // Assuming IndividualInfo and CompanyHeadquartersInfo are defined elsewhere in your project:
    // IndividualInfo should include personal identification data such as National ID, contact details, etc.
    // CompanyHeadquartersInfo should encompass address-related fields such as city, street, etc.

    // PastDateOnly custom validation attribute to ensure the date is in the past.
}
