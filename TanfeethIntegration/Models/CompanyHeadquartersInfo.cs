using System.ComponentModel.DataAnnotations;

namespace TanfeethIntegration.Models
{
    public class CompanyHeadquartersInfo
    {
        // Fixed value assumed for "المملكة العربية السعودية", other countries are selected by user.
        // من المفترض اختيار القيمة الثابتة لـ"المملكة العربية السعودية"، ويقوم المستخدمون باختيار الدول الأخرى.
        // [Required(ErrorMessage = "هذا الحقل مطلوب.")]
       // [Required]
        public int? CountryId { get; set; }

       // [Required(ErrorMessage = "حقل معرف المدينة مطلوب.")]
        public int? CityId { get; set; }
        public string? City { get; set; }

        [RegularExpression(@"^[\u0621-\u064A\s]+$", ErrorMessage = "يجب أن يحتوي المنطقة على حروف عربية فقط.")]
        public string? District { get; set; }

        [RegularExpression(@"^[\u0621-\u064A\s]+$", ErrorMessage = "يجب أن يحتوي الشارع على حروف عربية فقط.")]
        public string? Street { get; set; }

        [Range(1000, 9999, ErrorMessage = "يجب أن يكون رقم المبنى عددًا مكونًا من 4 أرقام.")]
        public int? BuildingNumber { get; set; }

        [Range(10000, 99999, ErrorMessage = "يجب أن يكون الرمز البريدي عددًا مكونًا من 5 أرقام.")]
        public int? PostCode { get; set; }

        [Range(1000, 9999, ErrorMessage = "يجب أن يكون الرقم الإضافي عددًا مكونًا من 4 أرقام.")]
        public int? AdditionalNumber { get; set; }

        [StringLength(150, ErrorMessage = "يجب ألا تتجاوز المعلومات الإضافية 150 حرفًا.")]
        public string? ExtraInfo { get; set; }
    }
}
