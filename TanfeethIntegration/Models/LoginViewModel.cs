using System.ComponentModel.DataAnnotations;

namespace TanfeethIntegration.Models
{
    public class LoginViewModel
    {
       
        
        [Required]
        [Display(Name = "اسم المستخدم")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }

        [Display(Name = "تذكرني")]
        public bool RememberMe { get; set; }
    }

}
