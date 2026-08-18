using System.ComponentModel.DataAnnotations;

namespace TanfeethIntegration.Models
{
    public class DefendantInformation
    {
        [Required]
        [EnumDataType(typeof(DefendantType))]
        public DefendantType DefendantTypeId { get; set; }

        // This list will include either a registered company info, unregistered company info, etc.
        // We would typically use inheritance here or break these out into separate models as needed.
        // For example, if DefendantTypeId is RegisteredCompany, we would expect to have RegisteredCompanyInfo filled in.
    }
}
