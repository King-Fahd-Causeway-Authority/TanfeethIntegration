using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TanfeethIntegration.Models
{
    public class AdministrativeAgencyInformation
    {
        [Required]
        public int AgencyId { get; set; }
     
        public IEnumerable<SelectListItem> Agencies { get; set; } = new List<SelectListItem>();




    }

}
