namespace TanfeethIntegration.Models
{
    public class RequestBodyModel
    {
        public LegalProceedingRequest LegalProceedingRequest { get; set; }
        public DefendantInformationModel DefendantInformationModel { get; set; }
        public ExecutionClaimData ExecutionClaimData { get; set; }
        public EnforcementInfo EnforcementInfo { get; set; }
    }
}
