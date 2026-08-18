using System.ComponentModel.DataAnnotations;

namespace TanfeethIntegration.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string? FullName { get; set; }

        public string Email { get; set; }
    }

}
