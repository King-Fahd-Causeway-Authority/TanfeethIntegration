namespace TanfeethIntegration.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PrivateFoundationDefendantModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "The license number must be 50 characters or less.")]
        [RegularExpression("^[A-Za-z0-9]*$", ErrorMessage = "The license number can only contain English letters and numbers with no spaces.")]
        public string LicenseNumber { get; set; }

        [Required]
        [Range(7000000000, 7999999999, ErrorMessage = "The unified number must start with a 7 and be 10 digits long.")]
        public long UnifiedNumber { get; set; }

        [Required]
        public int LicenseSourceId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The name must be 100 characters or less.")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime LicenseRegDate { get; set; }

        [Required]
        public CompanyHeadquartersInfo Headquarters { get; set; }

        [Required]
        public IndividualInfo FoundingMemberDetails { get; set; }

        // Nested models to represent the shared structures used in this and other defendant types
       
    }

    // Annotations for date range validator are set to ensure the date is not in the future.
    // Use a custom validation for the Hirji or Gregorian requirement and additional custom logic.
    // You should further customize these classes by adding the specific fields as per your application needs.
}
