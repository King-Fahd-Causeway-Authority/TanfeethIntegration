using System.ComponentModel.DataAnnotations;



    namespace TanfeethIntegration.Models
    {
    public class IndividualInfo
    {

        [Required(ErrorMessage = "هذا الحقل مطلوب.")]
        public int IdentityTypeId { get; set; }

        // BorderNumber: required for GulfCitizen and Visitor, conditional on the absence of GulfNumber for GulfCitizen,
        // and the absence of PassportNumber for Visitor.
        [ConditionalRequired("IdentityTypeId", new object[] { IdentityType.GulfCitizen },  "رقم الحدود أو رقم الهوية الخليجية اجباري.")]
        [ConditionalRequired("IdentityTypeId", new object[] { IdentityType.Visitor },  "رقم الحدود أو رقم جواز السفر اجباري.")]
        [RegularExpression(@"^(3|4)\d{9}$", ErrorMessage = "يجب أن يبدأ بالرقم 3 أو 4 ويكون مكون من 10 أرقام.")]
        public long? BorderNumber { get; set; }

        // GulfNumber: required for GulfCitizen, conditional on the absence of BorderNumber.
        [ConditionalRequired("IdentityTypeId", new object[] { IdentityType.GulfCitizen }, "رقم الهوية الخليجية مطلوب أو رقم الحدود.")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "يجب أن يكون رقم الهوية الخليجية ما بين 6 إلى 15 حرفاً.")]
        public string? GulfNumber { get; set; }

        // PassportNumber: required for Visitor, conditional on the absence of BorderNumber.
        [ConditionalRequired("IdentityTypeId", new object[] { IdentityType.Visitor }, "   رقم جواز السفر مطلوب أو رقم الحدود للزائر")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "يجب أن يكون رقم جواز السفر ما بين 6 إلى 15 حرفاً.")]
        public long? PassportNumber { get; set; }
        public long? VisaNumber { get; set; }



        // NationalityId: required for GulfCitizen and Visitor.
        [ConditionalRequired("IdentityTypeId", new object[] { IdentityType.GulfCitizen, IdentityType.Visitor }, "مطلوب اذا كانت الهوية من نوع زائر أو خليجي")]

        public int? NationalityId { get; set; }
        [ConditionalRequired("IdentityTypeId", new object[] { IdentityType.GulfCitizen, IdentityType.Visitor }, "مطلوب اذا كانت الهوية من نوع زائر أو خليجي")]

        public int? AbsherNationalityCode { get; set; }

        // BirthDate: required for GulfCitizen and Visitor.
        [ConditionalRequired("IdentityTypeId", new object[] { IdentityType.GulfCitizen, IdentityType.Visitor }, "مطلوب اذا كانت الهوية من نوع زائر أو خليجي")]
        [DataType(DataType.Date)]
        // Custom validation might be required to check if the date is Hijri or Gregorian and that it's in the past.
        public DateTime BirthDate { get; set; }

        //// VisitorOrGulfFullName: required for GulfCitizen and Visitor.
        //[ConditionalRequired("IdentityTypeId", new object[] { IdentityType.GulfCitizen, IdentityType.Visitor }, "الاسم الثلاثي مطلوب للخليجي أو الزائر.")]
        //[StringLength(100, ErrorMessage = "لا يمكن أن يتجاوز الاسم الكامل 100 حرف.")]
        //public string? VisitorOrGulfFullName { get; set; }
       
        [ConditionalRequired("IdentityTypeId", new object[] { IdentityType.GulfCitizen, IdentityType.Visitor }, "الاسم الثلاثي مطلوب للخليجي أو الزائر.")]
        [StringLength(100, ErrorMessage = "لا يمكن أن يتجاوز الاسم الكامل 100 حرف.")]
        public string? FirstName { get; set; }

        [ConditionalRequired("IdentityTypeId", new object[] { IdentityType.GulfCitizen, IdentityType.Visitor }, "الاسم الثلاثي مطلوب للخليجي أو الزائر.")]
        [StringLength(100, ErrorMessage = "لا يمكن أن يتجاوز الاسم الكامل 100 حرف.")]
        public string? FatherName { get; set; }

        [ConditionalRequired("IdentityTypeId", new object[] { IdentityType.GulfCitizen, IdentityType.Visitor }, "الاسم الثلاثي مطلوب للخليجي أو الزائر.")]
        [StringLength(100, ErrorMessage = "لا يمكن أن يتجاوز الاسم الكامل 100 حرف.")]
        public string? GrandfatherName { get; set; }

        [ConditionalRequired("IdentityTypeId", new object[] { IdentityType.GulfCitizen, IdentityType.Visitor }, "الاسم الثلاثي مطلوب للخليجي أو الزائر.")]
        [StringLength(100, ErrorMessage = "لا يمكن أن يتجاوز الاسم الكامل 100 حرف.")]
        public string? FamilyName { get; set; }

        // NationalID: required for National.
        [ConditionalRequired("IdentityTypeId", new object[] { IdentityType.National},  "  رقم الهوية الوطنية مطلوب للمواطن .")]
        [RegularExpression(@"^1\d{9}$", ErrorMessage = "يجب أن يبدأ رقم الهوية الوطنية بالرقم 1 ويكون مكون من 10 أرقام.")]
        public long? NationalID { get; set; }

        // IqamaNumber: required for Resident.
        [ConditionalRequired("IdentityTypeId", new object[] { IdentityType.Resident }, "رقم الإقامة مطلوب للمقيم .")]
        [RegularExpression(@"^2\d{9}$", ErrorMessage = "يجب أن يبدأ رقم الإقامة بالرقم 2 ويكون مكون من 10 أرقام.")]
        public long? IqamaNumber { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب.")]
        [RegularExpression(@"^05\d{8,9}$", ErrorMessage = "رقم الهاتف غير صحيح")] public string MobileNumber { get; set; }

            [Required(ErrorMessage = "هذا الحقل مطلوب.")]
            [EmailAddress(ErrorMessage = "الرجاء إدخال بريد إلكتروني صالح.")]
            public string Email { get; set; }

            // ...الخصائص الأخرى بناءً على البيانات المطلوبة إضافياً للأفراد
        }
    }

