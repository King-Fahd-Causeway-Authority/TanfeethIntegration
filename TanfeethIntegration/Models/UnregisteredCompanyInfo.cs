using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TanfeethIntegration.Models
{
    public class UnregisteredCompanyInfo
    {
        [StringLength(50, ErrorMessage = "يجب ألا يتجاوز رقم السجل التجاري 50 حرفًا.")]
        public string? CrNumber { get; set; }

        [DataType(DataType.Date)]
        [PastDateOnly(ErrorMessage = "يجب أن يكون تاريخ بداية السجل التجاري في الماضي أو تاريخ اليوم.")]
        public DateTime CrNumberStartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "تاريخ انتهاء السجل التجاري")]
        [DateGreaterThan(nameof(CrNumberStartDate), "يجب أن يكون تاريخ انتهاء السجل التجاري بعد تاريخ البداية.")]
        public DateTime CrNumberEndDate { get; set; }

        [Required(ErrorMessage = "اسم الشركة مطلوب.")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز اسم الشركة 100 حرف.")]
        public string CompanyName { get; set; }

        [Range(0, 999999999999999999, ErrorMessage = "يجب أن يكون رقم ترخيص ميسا بين 0 و 18 رقمًا.")]
        public long? MISALicenseNo { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression(@"^05\d{8,9}$", ErrorMessage = "رقم الهاتف غير صحيح")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب.")]
        [EmailAddress(ErrorMessage = "الرجاء إدخال بريد إلكتروني صالح.")]
        public string Email { get; set; }
        // تفاصيل العوامل
        // public AgentInfo AgentDetails { get; set; }

        // تفاصيل الأفراد إذا كانت هناك حاجة إلى معلومات أكثر تفصيلًا
        public IndividualInfo IndividualDetails { get; set; }

        // معلومات المقر الرئيسي للشركة
        public CompanyHeadquartersInfo Headquarters { get; set; }
    }
}
