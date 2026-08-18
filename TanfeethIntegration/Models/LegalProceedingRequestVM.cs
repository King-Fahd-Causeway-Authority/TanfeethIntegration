using Microsoft.AspNetCore.Mvc.Rendering;

namespace TanfeethIntegration.Models
{
    public class LegalProceedingRequestVM
    {
        public LegalProceedingRequest LegalProceedingRequest { get; set; }
        public SelectList CourtOptions { get; set; }
        public SelectList AgencyOptions { get; set; }
        public int CurrentStep { get; set; }
        public LegalProceedingRequestVM()
        {
            LegalProceedingRequest = new LegalProceedingRequest();
            CurrentStep = 1; // Use number sequencing for steps
                             // Initialize other properties here if needed.
        }

    }
}
