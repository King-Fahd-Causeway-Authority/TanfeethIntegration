using System.ComponentModel.DataAnnotations.Schema;

namespace TanfeethIntegration.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class LegalProceedingRequest
    {
        [Required(ErrorMessage = "هذا الحقل مطلوب.")]
        public int CourtId { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب.")]
        //public int AgencyId { get; set; }

        public bool IsUrgent { get; set; }
        public SelectList CourtOptions { get; set; }
        //public SelectList AgencyOptions { get; set; }

        public LegalRepresentative LegalRepresentative  { get; set; }



    }




}
