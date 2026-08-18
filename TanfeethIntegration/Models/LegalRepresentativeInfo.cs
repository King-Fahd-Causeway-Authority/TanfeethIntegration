using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TanfeethIntegration.Models
{
    //public class LegalRepresentativeInfo
    //{
    //    // The identifier for the agency this legal representative is associated with.
    //    [Required]
    //    public int AgencyId { get; set; }

    //    // Arabic name of the agency; populated via a lookup mechanism and not stored in the database.
    //    [NotMapped]
    //    public string AgencyNameAr { get; set; }

    //    // English name of the agency; populated via a lookup mechanism and not stored in the database.
    //    [NotMapped]
    //    public string AgencyNameEn { get; set; }

    //    // The identifier for the legal representative.
    //    [Required]
       
    //    public LegalRepresentative LegalRepresentative { get; set; }


    //    // A method or process can populate AgencyNameAr and AgencyNameEn based on the AgencyId.
    //    // This might happen within a service that calls the lookup endpoint and retrieves the agency information.

    //}

    //// Interface for a service that performs lookups.

    //// A class representing the detailed information of an agency, usually obtained through a lookup.
    //public class AgencyInfo
    //{
    //    public int Id { get; set; }
    //    public string NameAr { get; set; }
    //    public string NameEn { get; set; }
    //}
}
