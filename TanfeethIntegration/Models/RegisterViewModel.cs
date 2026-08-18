using System.ComponentModel.DataAnnotations;

namespace TanfeethIntegration.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "الاسم الكامل")]
        [RegularExpression(@"^[a-zA-Z0-9\u0621-\u064A\s]+$", ErrorMessage = "Invalid characters in full name.")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "اسم المستخدم")]
        [RegularExpression(@"^[a-zA-Z0-9\u0621-\u064A\s]+(\.[a-zA-Z0-9\u0621-\u064A\s]+)*$", ErrorMessage = "Invalid characters in username.")]
        public string Username { get; set; }


        public bool IsActive { get; set; } = true; // Default to true
        public bool IsAdmin { get; set; } = false; // Default to false
    }

}
