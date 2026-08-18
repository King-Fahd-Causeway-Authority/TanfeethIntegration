using System.ComponentModel.DataAnnotations;

namespace TanfeethIntegration.Models
{
    public class DefendantInformationModel
    {
        public int DefendantTypeId { get; set; }
        public RegisteredCompanyInfo RegisteredCompanyInfo { get; set; }    

        public UnregisteredCompanyInfo UnregisteredCompanyInfo { get; set; }

        public CharitableAssociationInfo CharitableAssociationInfo { get; set; }

        public CivilOrganizationInfo CivilOrganizationInfo { get; set;}

        public IndividualInfoType IndividualInfoType { get; set; }  

        public EndowmentInfo EndowmentInfo { get; set; }
        public AdministrativeAgencyInformation AdministrativeAgencyInformation { get; set; }
        

          
        



    }
}
