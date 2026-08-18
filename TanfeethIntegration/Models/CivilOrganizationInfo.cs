namespace TanfeethIntegration.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CivilOrganizationInfo
    {
        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "License number must contain only English letters and numbers with no spaces.")]
        public string LicenseNumber { get; set; }

        [Required]
        [RegularExpression(@"^7\d{9}$", ErrorMessage = "Unified number must start with 7 and be 10 digits long.")]
        public long UnifiedNumber { get; set; }
        [Required(ErrorMessage = "مصدر الرخصة مطلوب.")]
        public int LicenseSourceId { get; set; }

        [Required]
        public IEnumerable<SelectListItem> LicenseSources { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [PastDate(ErrorMessage = "License registration date must be less than today's date.")]
        public DateTime LicenseRegDate { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        public string Email { get; set; }
        [Required(ErrorMessage = "رقم الهاتف مطلوب")]

        [RegularExpression(@"^05\d{8,9}$", ErrorMessage = "رقم الهاتف غير صحيح")]
        public string PhoneNumber { get; set; }

        // Assuming we have a separate model for address called 'AddressInfo'.
        [Required]
        public CompanyHeadquartersInfo CompanyHeadquarters { get; set; }

        // Assuming we have a separate model for individual information for the founding member.
        [Required]
        public IndividualInfo FoundingMemberInfo { get; set; }
    }
}
