namespace TanfeethIntegration.DTOs
{
    using System;
    using System.Collections.Generic;

    public class Address
    {
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public string CityText { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public int? BuildingNumber { get; set; }
        public int? PostCode { get; set; }
        public int? AdditionalNumber { get; set; }
        public string ExtraInfo { get; set; }
    }

    public class Manager
    {
        public BirthDate BirthDate { get; set; }
        public string SaudiMobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public int IdentityTypeId { get; set; }
        public long? NationalId { get; set; }
        public long? IqamaNumber { get; set; }
        public string FirstName { get; set; }
        public string FatherName { get; set; }
        public string GrandfatherName { get; set; }
        public string FamilyName { get; set; }
        public long? NationalityId { get; set; }
        public long? BorderNumber { get; set; }
        public long? VisaNumber { get; set; }
        public long? passporttNumber { get; set; }
    }

    public class Person
    {
        public BirthDate BirthDate { get; set; }
        public string SaudiMobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public int IdentityTypeId { get; set; }
        public long? NationalId { get; set; }
        public long? IqamaNumber { get; set; }
        public string FirstName { get; set; }
        public string FatherName { get; set; }
        public string GrandfatherName { get; set; }
        public string FamilyName { get; set; }
        public long? NationalityId { get; set; }
        public long? BorderNumber { get; set; }
        public long? VisaNumber { get; set; }
        public long? passporttNumber { get; set; }
    }

    public class RegisteredCompanyDefendant
    {
        public string Name { get; set; }
        public Address Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public CrManager CrManager { get; set; }
        public long? NationalUnifiedNumber { get; set; }
        public long CommercialRegistrationNumber { get; set; }
        public BirthDate CrStartDate { get; set; }
        public BirthDate CrEndDate { get; set; }
    }
    public class WaqfDefendant
    {
        public string Name { get; set; }
        public Address Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public string EndowmentRegNumber { get; set; }
        public string DeedNumber { get; set; }
        public BirthDate DeedRegDate { get; set; }

        public WaqfSuperintendent WaqfSuperintendent { get; set; }
    }

    public class charityDefendant
    {
        public string LicenseNumber { get; set; }

        public long UnifiedNumber { get; set; }

        public int LicenseSourceId { get; set; }

        public string Name { get; set; }

        public BirthDate LicenseRegistrationDate { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
        public Address Address { get; set; }
        public Manager  Manager { get; set; }
    }


    public class nonProfitDefendant
    {
        public string LicenseNumber { get; set; }

        public long UnifiedNumber { get; set; }

        public int LicenseSourceId { get; set; }

        public string Name { get; set; }

        public BirthDate LicenseRegistrationDate { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
        public Address Address { get; set; }
        public FoundingMember FoundingMember { get; set; }
    }

    public class AgencyDefendant
    {
        public int AgencyId { get; set; }
    }
    public class IndividualDefendant
    {
        public Address Address { get; set; }
        public Person Person { get; set; }
    }

    public class DefendantWrapper
    {
        public RegisteredCompanyDefendant RegisteredCompanyDefendant { get; set; }
        public UnRegisteredCompanyDefendant  unRegisteredCompanyDefendant { get; set; }

        public IndividualDefendant IndividualDefendant { get; set; }
        public WaqfDefendant WaqfDefendant { get; set; }
        public charityDefendant charityDefendant { get; set; }
        public nonProfitDefendant nonProfitDefendant { get; set; }
        public AgencyDefendant AgencyDefendant  { get; set; }



    }
    public class UnRegisteredCompanyDefendant
    {
        public string CrNumber { get; set; }

        public BirthDate StartDate { get; set; }

        public BirthDate EndDate { get; set; }

        public string Name { get; set; }

        public long? MISALicenseNo { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
        public Address Address { get; set; }
        public representative representative { get; set; }

    }

    public class EnforcementFile
    {
        public string FileName { get; set; }
        public string Base64Content { get; set; }
    }

    public class EnforcementItem
    {
        public string Data { get; set; }
    }

    public class Enforcement
    {
        public int TypeId { get; set; }
        public string Number { get; set; }
        public BirthDate Date { get; set; }
        public string Issuer { get; set; }
        public string IssuePlace { get; set; }
        public bool HasFinancialClaims { get; set; }
        public decimal Amount { get; set; }
        public string AmountText { get; set; }
        public int CurrencyId { get; set; }
        public string PlaintiffIBANNumber { get; set; }
        public string Statement { get; set; }
        public List<EnforcementItem> EnforcementItems { get; set; }
        public EnforcementFile EnforcementFile { get; set; }
        public object EnforcementExtraFiles { get; set; }
    }

    public class ExecutionClaimFile
    {
        public string FileName { get; set; }
        public string Base64Content { get; set; }
    }

    public class ExecutionClaim
    {
        public BirthDate Date { get; set; }
        public int ResultId { get; set; }
        public string RequestNumber { get; set; }
        public ExecutionClaimFile ExecutionClaimFile { get; set; }
        public ExecutionClaimFile ProofOfSubmitFile { get; set; }
    }

    public class Plaintiff
    {
        public int AgencyId { get; set; }
        public long LegalRepIdentityId { get; set; }
    }

    public class GeneralInformation
    {
        public int CourtId { get; set; }
        public bool IsUrgent { get; set; }
    }

    public class RequestModel
    {
        public GeneralInformation GeneralInformation { get; set; }
        public Plaintiff Plaintiff { get; set; }
        public List<DefendantWrapper> Defendants { get; set; }
        public Enforcement Enforcement { get; set; }
        public ExecutionClaim ExecutionClaim { get; set; }
    }

    				
        public class representative
    {
        public BirthDate BirthDate { get; set; }
        public string SaudiMobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public int IdentityTypeId { get; set; }
        public long? NationalId { get; set; }
        public long? IqamaNumber { get; set; }
        public string FirstName { get; set; }
        public string FatherName { get; set; }
        public string GrandfatherName { get; set; }
        public string FamilyName { get; set; }
        public long? NationalityId { get; set; }
        public long? BorderNumber { get; set; }
        public long? VisaNumber { get; set; }
        public long? passporttNumber { get; set; }


    }

    public class FoundingMember
    {
        public BirthDate BirthDate { get; set; }
        public string SaudiMobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public int IdentityTypeId { get; set; }
        public long? NationalId { get; set; }
        public long? IqamaNumber { get; set; }
        public string FirstName { get; set; }
        public string FatherName { get; set; }
        public string GrandfatherName { get; set; }
        public string FamilyName { get; set; }
        public long? NationalityId { get; set; }
        public long? BorderNumber { get; set; }
        public long? VisaNumber { get; set; }
        public long? passporttNumber { get; set; }


    }

    public class CrManager
    {
        public BirthDate BirthDate { get; set; }
        public string SaudiMobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public int IdentityTypeId { get; set; }
        public long? NationalId { get; set; }
        public long? IqamaNumber { get; set; }
        public string FirstName { get; set; }
        public string FatherName { get; set; }
        public string GrandfatherName { get; set; }
        public string FamilyName { get; set; }
        public long? NationalityId { get; set; }
        public long? BorderNumber { get; set; }
        public long? VisaNumber { get; set; }
        public long? passporttNumber { get; set; }

        
    }

    public class WaqfSuperintendent
    {
        public BirthDate BirthDate { get; set; }
        public string SaudiMobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public int IdentityTypeId { get; set; }
        public long? NationalId { get; set; }
        public long? IqamaNumber { get; set; }
        public string FirstName { get; set; }
        public string FatherName { get; set; }
        public string GrandfatherName { get; set; }
        public string FamilyName { get; set; }
        public long? NationalityId { get; set; }
        public long? BorderNumber { get; set; }
        public long? VisaNumber { get; set; }
        public long? passporttNumber { get; set; }


    }

    public class BirthDate
    {
        public string Gregorian { get; set; }
        public object Hijri { get; set; }
    }
  




}
