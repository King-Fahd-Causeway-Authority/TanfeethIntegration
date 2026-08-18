using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TanfeethIntegration.Models
{
    public class LegalProceedingViewModel
    {
        
        // Holds information specific to the legal proceeding request. 
        // This could include details like case type, court details, etc.
        public LegalProceedingRequest LegalProceedingRequest { get; set; }

        public DefendantInformationModel DefendantInformationModel { get; set; }

        public ExecutionClaimData ExecutionClaimData { get; set; }

        // Information pertaining to the enforcement of legal judgments or orders.
        public EnforcementInfo EnforcementInfo { get; set; }

        

        // Constructor initializes the first step by default and sets up initial state for other properties, if necessary.
        public LegalProceedingViewModel()
        {
            //LegalProceedingRequest = new LegalProceedingRequest();
            //ExecutionClaimData= new ExecutionClaimData();
            DefendantInformationModel= new DefendantInformationModel();
            DefendantInformationModel.DefendantTypeId = 1;
            DefendantInformationModel.RegisteredCompanyInfo = new RegisteredCompanyInfo();
            DefendantInformationModel.UnregisteredCompanyInfo = new UnregisteredCompanyInfo();
            DefendantInformationModel.CharitableAssociationInfo = new CharitableAssociationInfo();
            DefendantInformationModel.CivilOrganizationInfo = new CivilOrganizationInfo();
            DefendantInformationModel.EndowmentInfo = new EndowmentInfo();
            DefendantInformationModel.IndividualInfoType = new IndividualInfoType();
            DefendantInformationModel.AdministrativeAgencyInformation= new AdministrativeAgencyInformation();
        }
    }
}
