using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TanfeethIntegration.Models
{
    public class CharitableAssociationInfo
    {
        [Required(ErrorMessage = "رقم الرخصة مطلوب.")]
        [StringLength(50, ErrorMessage = "يجب ألا يتجاوز رقم الرخصة 50 حرفًا.")]
        [RegularExpression("^[A-Za-z0-9]+$", ErrorMessage = "يمكن أن يحتوي رقم الرخصة على الحروف الإنجليزية والأرقام فقط بدون فراغات.")]
        public string LicenseNumber { get; set; }

        [Required(ErrorMessage = "الرقم الموحد مطلوب.")]
        [RegularExpression("^7\\d{9}$", ErrorMessage = "يجب أن يبدأ الرقم الموحد بـ '7' ويتكون من 10 أرقام.")]
        public long UnifiedNumber { get; set; }

        [Required(ErrorMessage = "مصدر الرخصة مطلوب.")]
        public int LicenseSourceId { get; set; }

        public IEnumerable<SelectListItem> LicenseSources { get; set; }

        [Required(ErrorMessage = "اسم الجمعية مطلوب.")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز اسم الجمعية 100 حرف.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "تاريخ تسجيل الرخصة مطلوب.")]
        [DataType(DataType.Date)]
        [PastDate(ErrorMessage = "يجب أن يكون تاريخ تسجيل الرخصة أقل من تاريخ اليوم.")]
        public DateTime LicenseRegDate { get; set; }
       
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        public string Email { get; set; }
        [Required(ErrorMessage = "رقم الهاتف مطلوب")]

        [RegularExpression(@"^05\d{8,9}$", ErrorMessage = "رقم الهاتف غير صحيح")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "معلومات الفرد مطلوبة.")]
        public IndividualInfo IndividualInfo { get; set; } // Reference to individual information

        [Required(ErrorMessage = "معلومات العضو المؤسس مطلوبة.")]
        public CompanyHeadquartersInfo CompanyHeadquarters { get; set; }
    }
}
