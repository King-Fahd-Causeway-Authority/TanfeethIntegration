using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using TanfeethIntegration.DTOs;
using TanfeethIntegration.Models;
using TanfeethIntegration.Services;

namespace TanfeethIntegration.Controllers
{
    [UserExistsAuthorize]
    public class LegalProceedingController : Controller
    {
        private readonly ILogger<LegalProceedingController> _logger;
        private readonly IGovAgencyRequestService _govAgencyRequestService; // Assumed service interface
        private readonly ILookupService _lookupService;


        public LegalProceedingController(ILogger<LegalProceedingController> logger, IGovAgencyRequestService govAgencyRequestService,
            ILookupService lookupService)

        {
            _logger = logger;
            _govAgencyRequestService = govAgencyRequestService;
            _lookupService = lookupService;
        }
        public async Task<IActionResult> AdministrativeAgencyInformation()
        {
            var agencies = await _lookupService.GetAgenciesAsync();

            if (agencies.Any())
            {
                var agencyNames = agencies.Select(agency => new SelectListItem
                {
                    Value = agency.Id.ToString(),
                    Text = agency.name
                }).ToList();

                var model = new AdministrativeAgencyInformation
                {
                    Agencies = new SelectList(agencyNames, "Value", "Text")
                };

                return PartialView(model);
            }

            // Handle the case when there are no agencies
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var EnforcementTypes = await _lookupService.GetEnforcementTypesAsync(); //GetCitesSelectList();

            if (EnforcementTypes.Any())
            {
                // Extract the city names
                var EnforcementTypesNames = EnforcementTypes.Select(EnforcementType => new SelectListItem
                {
                    Value = EnforcementType.Id.ToString(),    // Adjust property names as needed
                    Text = EnforcementType.NameAr             // Adjust property names as needed
                }).ToList();

                // Store the city names in ViewData or pass it to the view model, depending on your preference
                ViewData["EnforcementTypeId"] = new SelectList(EnforcementTypesNames, "Value", "Text");
            }
            else
            {
                // Handle the error case if needed
                Console.WriteLine("API Error: Unable to retrieve city data");
                ViewData["EnforcementTypeId"] = new SelectList(new List<string>()); // Empty list or handle the error accordingly
            }


            var ExcutioncliamResults = await _lookupService.GetExecutionClaimResultsAsync(); //GetCitesSelectList();

            if (ExcutioncliamResults.Any())
            {
                // Extract the city names
                var ExcutioncliamResultsNames = ExcutioncliamResults.Select(ExcutioncliamResult => new SelectListItem
                {
                    Value = ExcutioncliamResult.Id.ToString(),    // Adjust property names as needed
                    Text = ExcutioncliamResult.nameAr             // Adjust property names as needed
                }).ToList();

                // Store the city names in ViewData or pass it to the view model, depending on your preference
                ViewData["ExcutioncliamResultsId"] = new SelectList(ExcutioncliamResultsNames,"Value", "Text");
            }
            else
            {
                // Handle the error case if needed
                Console.WriteLine("API Error: Unable to retrieve city data");
                ViewData["ExcutioncliamResultsId"] = new SelectList(new List<string>()); // Empty list or handle the error accordingly
            }

            // Sample static data


            

            var agencies = await _lookupService.GetAgenciesAsync(); //GetCitesSelectList();

          
            if (agencies.Any())
            {
                // Extract the city names
                var agencNames = agencies.Select(agency => new SelectListItem
                {
                    Value = agency.Id.ToString(),    // Adjust property names as needed
                    Text = agency.name             // Adjust property names as needed
                }).ToList();

                // Store the city names in ViewData or pass it to the view model, depending on your preference
                ViewData["AgencyId"] = new SelectList(agencNames, "Value", "Text");
            }
            else
            {
                // Handle the error case if needed
                Console.WriteLine("API Error: Unable to retrieve city data");
                ViewData["AgencyId"] = new SelectList(new List<string>()); // Empty list or handle the error accordingly
            }

            var Courts = await _lookupService.GetCourtsAsync(); //GetCitesSelectList();

            if (Courts.Any())
            {
                var CourtNames = Courts.Select(court => new SelectListItem
                {
                    Value = court.id.ToString(),    // Adjust property names as needed
                    Text = court.nameAr             // Adjust property names as needed
                }).ToList();

                
               

                // Store the list in ViewData
                ViewData["CourtId"] = new SelectList(CourtNames, "Value", "Text");
            }
            else
            {
                // Handle the error case if needed
                ViewData["CourtId"] = new SelectList(new List<string>()); // Empty list or handle the error accordingly
            }

            var IdentityTypes = await _lookupService.GetIdentityTypesAsync(); //GetCitesSelectList();

            if (IdentityTypes.Any())
            {
                // Extract the city names
                var IdentityTypenames = IdentityTypes.Select(IdentityType => new SelectListItem
                {
                    Value = IdentityType.Id.ToString(),    // Adjust property names as needed
                    Text = IdentityType.nameAr             // Adjust property names as needed
                }).ToList();

                // Store the city names in ViewData or pass it to the view model, depending on your preference
                ViewData["IdentityTypeId"] = new SelectList(IdentityTypenames, "Value", "Text");
            }
            else
            {
                // Handle the error case if needed
                Console.WriteLine("API Error: Unable to retrieve city data");
                ViewData["IdentityTypeId"] = new SelectList(new List<string>()); // Empty list or handle the error accordingly
            }
                var viewModel = new LegalProceedingViewModel
            {
                LegalProceedingRequest = new LegalProceedingRequest
                {

                    

                    LegalRepresentative = new LegalRepresentative()
                    {
                    }


                },
                DefendantInformationModel = new DefendantInformationModel
                {
                    CivilOrganizationInfo = new CivilOrganizationInfo
                    {
                        //LicenseSources = new SelectList(GetLicenseSources(), "Id", "NameAr")

                    },
                    CharitableAssociationInfo = new CharitableAssociationInfo
                    {
                        //LicenseSources = new SelectList(GetLicenseSources(), "Id", "NameAr")

                    }
                },
               


            };


            return View(viewModel);
        }
        // This endpoint handles the data for different steps in the form sequence
        [HttpPost]
        public async Task<IActionResult> Index([FromForm] LegalProceedingViewModel viewModel)
        {

            string executionClaimFileBase64 = viewModel.ExecutionClaimData.ExecutionClaimFile != null
                        ? await ConvertFileToBase64(viewModel.ExecutionClaimData.ExecutionClaimFile)
                        : string.Empty;
            string proofOfSubmitFileBase64 = viewModel.ExecutionClaimData.ProofOfSubmitFile != null
                ? await ConvertFileToBase64(viewModel.ExecutionClaimData.ProofOfSubmitFile)
                : string.Empty;
            string enforcementFileBase64 = viewModel.EnforcementInfo.EnforcementFile != null
                ? await ConvertFileToBase64(viewModel.EnforcementInfo.EnforcementFile)
                : string.Empty;
            List<string> enforcementExtraFilesBase64 = viewModel.EnforcementInfo.EnforcementExtraFiles != null
                ? await ConvertFilesToBase64(viewModel.EnforcementInfo.EnforcementExtraFiles)
                : new List<string>();
            string deedDocumentFileBase64 = viewModel.LegalProceedingRequest.LegalRepresentative.DeedDocumentFile != null
                ? await ConvertFileToBase64(viewModel.LegalProceedingRequest.LegalRepresentative.DeedDocumentFile)
                : string.Empty;
            // Call the updated 
            var generalInformation = viewModel.LegalProceedingRequest;
          var  Plaintiff = generalInformation.LegalRepresentative;
            var Defendants = viewModel.DefendantInformationModel;
            var RegisteredCompanyDefendant = Defendants.RegisteredCompanyInfo;
            var IndividualDefendant = Defendants.IndividualInfoType;
           var IndividualDefendantAddress= IndividualDefendant.CompanyHeadquarters;
            var Person = IndividualDefendant.IndividualInfo;
            var waqfDefendant = Defendants.EndowmentInfo;
            var WaqfSuperintendent = waqfDefendant.CaretakerInfo;
            var waqfDefendantAddress = waqfDefendant.HeadquartersInfo;
            var Address =   RegisteredCompanyDefendant.CompanyHeadquarters;
            var CrManager = RegisteredCompanyDefendant.ManagerInfo;
            var charityDefendant = Defendants.CharitableAssociationInfo;
            var charityDefendantAddress = charityDefendant.CompanyHeadquarters;
            var Manager = charityDefendant.IndividualInfo;
            var nonProfitDefendant = Defendants.CivilOrganizationInfo;
            var nonProfitDefendantAddress= nonProfitDefendant.CompanyHeadquarters;
            var FoundingMember = nonProfitDefendant.FoundingMemberInfo;
            var AgencyDefendant = Defendants.AdministrativeAgencyInformation;
            var unRegisteredCompanyDefendant = Defendants.UnregisteredCompanyInfo;
            var unRegisteredCompanyDefendantAddreess = unRegisteredCompanyDefendant.Headquarters;
            var representative = unRegisteredCompanyDefendant.IndividualDetails;

            var ExecutionClaim = viewModel.ExecutionClaimData;
            string ExecutionClaimFileName = ExecutionClaim?.ExecutionClaimFile?.FileName ?? "ExecutionClaimFileName.pdf";
            string ProofOfSubmitFileName = ExecutionClaim?.ProofOfSubmitFile?.FileName ?? "ProofOfSubmitFileName.pdf";

            var Enforcement = viewModel.EnforcementInfo;
            string EnforcementFileName = Enforcement?.EnforcementFile?.FileName ?? "EnforcementFileName.pdf";
            string ExEnforcementFileName = Enforcement?.EnforcementExtraFiles?.FirstOrDefault()?.FileName ?? "DefaultFileName.pdf";


            RequestModel CreateExampleRequest()
            {
                var requestModel = new RequestModel
                {
                    GeneralInformation = new GeneralInformation
                    {
                        CourtId = generalInformation.CourtId,
                        IsUrgent = generalInformation?.IsUrgent ?? false
                    },
                    Plaintiff = new Plaintiff
                    {
                        AgencyId = Plaintiff.AgencyId,
                        LegalRepIdentityId = Plaintiff?.LegalRepIdentity ?? 0
                    },
                    Defendants = new List<DefendantWrapper>
        {
            new DefendantWrapper
            {
                RegisteredCompanyDefendant = RegisteredCompanyDefendant.CompanyHeadquarters!= null
                    ? new RegisteredCompanyDefendant
                    {
                        Name = RegisteredCompanyDefendant.CompanyName,
                        Address = Address != null
                            ? new Address
                            {
                                CountryId = Address.CountryId,
                                CityId = Address.CityId,
                                CityText = Address.City,
                                District = Address.District,
                                Street = Address.Street,
                                BuildingNumber = Address.BuildingNumber,
                                PostCode = Address.PostCode,
                                AdditionalNumber = Address.AdditionalNumber,
                                ExtraInfo = Address.ExtraInfo
                            }
                            : null,
                        PhoneNumber = RegisteredCompanyDefendant.PhoneNumber,
                        Email = RegisteredCompanyDefendant.Email,
                        CrManager = CrManager != null
                            ? new CrManager
                            {
                                BirthDate = CrManager.BirthDate != null
                                    ? new BirthDate
                                    {
                                        Gregorian = CrManager.BirthDate.ToString("yyyy-MM-dd")
                                    }
                                    : null,
                                passporttNumber = CrManager.PassportNumber,
                                SaudiMobileNumber = CrManager.MobileNumber,
                                EmailAddress = CrManager.Email,
                                IdentityTypeId = CrManager.IdentityTypeId,
                                NationalId = CrManager.NationalID,
                                IqamaNumber = CrManager.IqamaNumber,
                                FirstName = CrManager.FirstName,
                                FatherName = CrManager.FatherName,
                                GrandfatherName = CrManager.GrandfatherName,
                                FamilyName = CrManager.FamilyName,
                                NationalityId = CrManager.NationalityId,
                                BorderNumber = CrManager.BorderNumber,
                                VisaNumber = CrManager.VisaNumber
                            }
                            : null,

                        NationalUnifiedNumber = RegisteredCompanyDefendant.NationalUnifiedNumber,
                        CommercialRegistrationNumber = RegisteredCompanyDefendant.CommercialRegistrationNumber,
                        CrStartDate = RegisteredCompanyDefendant.CrNumberStartDate != null
                            ? new BirthDate
                            {
                                Gregorian = RegisteredCompanyDefendant.CrNumberStartDate.ToString("yyyy-MM-dd")
                            }
                            : null,
                        CrEndDate = RegisteredCompanyDefendant.CrNumberEndDate != null
                            ? new BirthDate
                            {
                                Gregorian = RegisteredCompanyDefendant.CrNumberEndDate.ToString("yyyy-MM-dd")
                            }
                            : null
                    }
                    : null,
               
                IndividualDefendant = IndividualDefendant.CompanyHeadquarters != null
                    ? new IndividualDefendant
                    {
                        Address = IndividualDefendantAddress != null
                            ? new Address
                            {
                                CountryId = IndividualDefendantAddress.CountryId,
                                CityId = IndividualDefendantAddress.CityId,
                                CityText = IndividualDefendantAddress.City,
                                District = IndividualDefendantAddress.District,
                                Street = IndividualDefendantAddress.Street,
                                BuildingNumber = IndividualDefendantAddress.BuildingNumber,
                                PostCode = IndividualDefendantAddress.PostCode,
                                AdditionalNumber = IndividualDefendantAddress.AdditionalNumber,
                                ExtraInfo = IndividualDefendantAddress.ExtraInfo
                            }
                            : null,
                        Person = Person != null
                            ? new Person
                            {
                                BirthDate = Person.BirthDate != null
                                    ? new BirthDate
                                    {
                                        Gregorian = Person.BirthDate.ToString("yyyy-MM-dd")
                                    }
                                    : null,
                                passporttNumber = Person.PassportNumber,
                                SaudiMobileNumber = Person.MobileNumber,
                                EmailAddress = Person.Email,
                                IdentityTypeId = Person.IdentityTypeId,
                                NationalId = Person.NationalID,
                                IqamaNumber = Person.IqamaNumber,
                                FirstName = Person.FirstName,
                                FatherName =Person.FatherName,
                                GrandfatherName = Person.GrandfatherName,
                                FamilyName = Person.FamilyName,
                                NationalityId = Person.NationalityId,
                                BorderNumber = Person.BorderNumber,
                                VisaNumber = Person.VisaNumber
                            }
                            : null
                    }
                    : null,
                 WaqfDefendant= waqfDefendant.HeadquartersInfo != null
                    ? new WaqfDefendant
                    {

                    EndowmentRegNumber=waqfDefendant.EndowmentRegNumber ,
                    DeedNumber=waqfDefendant.DeedNumber,
                    Name=waqfDefendant.Name,
                    DeedRegDate=waqfDefendant.DeedRegDate != null
                                    ? new BirthDate
                                    {
                                        Gregorian = waqfDefendant.DeedRegDate.ToString("yyyy-MM-dd")
                                    }
                                    : null,
                    PhoneNumber=waqfDefendant.PhoneNumber,
                    Email=waqfDefendant.Email,

                    Address = waqfDefendantAddress != null
                            ? new Address
                            {
                                CountryId = waqfDefendantAddress.CountryId,
                                CityId = waqfDefendantAddress.CityId,
                                CityText = waqfDefendantAddress.City,
                                District = waqfDefendantAddress.District,
                                Street = waqfDefendantAddress.Street,
                                BuildingNumber = waqfDefendantAddress.BuildingNumber,
                                PostCode = waqfDefendantAddress.PostCode,
                                AdditionalNumber = waqfDefendantAddress.AdditionalNumber,
                                ExtraInfo = waqfDefendantAddress.ExtraInfo
                            }
                            : null,
                        WaqfSuperintendent = WaqfSuperintendent != null
                            ? new WaqfSuperintendent
                            {
                                BirthDate = WaqfSuperintendent.BirthDate != null
                                    ? new BirthDate
                                    {
                                        Gregorian = WaqfSuperintendent.BirthDate.ToString("yyyy-MM-dd")
                                    }
                                    : null,
                                passporttNumber = WaqfSuperintendent.PassportNumber,
                                SaudiMobileNumber = WaqfSuperintendent.MobileNumber,
                                EmailAddress = WaqfSuperintendent.Email,
                                IdentityTypeId = WaqfSuperintendent.IdentityTypeId,
                                NationalId = WaqfSuperintendent.NationalID,
                                IqamaNumber = WaqfSuperintendent.IqamaNumber,
                                FirstName = WaqfSuperintendent.FirstName,
                                FatherName = WaqfSuperintendent.FatherName,
                                GrandfatherName = WaqfSuperintendent.GrandfatherName,
                                FamilyName = WaqfSuperintendent.FamilyName,
                                NationalityId = WaqfSuperintendent.NationalityId,
                                BorderNumber = WaqfSuperintendent.BorderNumber,
                                VisaNumber = WaqfSuperintendent.VisaNumber
                            }
                            : null


                    }:null,
                 charityDefendant=charityDefendant.CompanyHeadquarters != null
                    ? new charityDefendant
                    {
                            LicenseNumber=charityDefendant.LicenseNumber,
                            UnifiedNumber=charityDefendant.UnifiedNumber,
                            LicenseSourceId=charityDefendant.LicenseSourceId,
                            Name=charityDefendant.Name,
                            LicenseRegistrationDate=charityDefendant.LicenseRegDate != null
                                    ? new BirthDate
                                    {
                                        Gregorian = charityDefendant.LicenseRegDate.ToString("yyyy-MM-dd")
                                    }
                                    : null,
                            PhoneNumber=charityDefendant.PhoneNumber,
                            Email=charityDefendant.Email,
                        Address = charityDefendantAddress != null
                            ? new Address
                            {
                                CountryId = charityDefendantAddress.CountryId,
                                CityId = charityDefendantAddress.CityId,
                                CityText = charityDefendantAddress.City,
                                District = charityDefendantAddress.District,
                                Street = charityDefendantAddress.Street,
                                BuildingNumber = charityDefendantAddress.BuildingNumber,
                                PostCode = charityDefendantAddress.PostCode,
                                AdditionalNumber = charityDefendantAddress.AdditionalNumber,
                                ExtraInfo = charityDefendantAddress.ExtraInfo
                            }
                            : null,
                        Manager = Manager != null
                            ? new Manager
                            {
                                BirthDate = Manager.BirthDate != null
                                    ? new BirthDate
                                    {
                                        Gregorian = Manager.BirthDate.ToString("yyyy-MM-dd")
                                    }
                                    : null,
                                passporttNumber = Manager.PassportNumber,
                                SaudiMobileNumber = Manager.MobileNumber,
                                EmailAddress = Manager.Email,
                                IdentityTypeId = Manager.IdentityTypeId,
                                NationalId = Manager.NationalID,
                                IqamaNumber = Manager.IqamaNumber,
                                FirstName = Manager.FirstName,
                                FatherName = Manager.FatherName,
                                GrandfatherName = Manager.GrandfatherName,
                                FamilyName = Manager.FamilyName,
                                NationalityId = Manager.NationalityId,
                                BorderNumber = Manager.BorderNumber,
                                VisaNumber = Manager.VisaNumber
                            }
                            : null
                    }
                    : null,
                 nonProfitDefendant=nonProfitDefendant.CompanyHeadquarters != null
                    ? new nonProfitDefendant
                    {
                            LicenseNumber=nonProfitDefendant.LicenseNumber,
                            UnifiedNumber=nonProfitDefendant.UnifiedNumber,
                            LicenseSourceId=nonProfitDefendant.LicenseSourceId,
                            Name=nonProfitDefendant.Name,
                            LicenseRegistrationDate=nonProfitDefendant.LicenseRegDate != null
                                    ? new BirthDate
                                    {
                                        Gregorian = nonProfitDefendant.LicenseRegDate.ToString("yyyy-MM-dd")
                                    }
                                    : null,
                            PhoneNumber=nonProfitDefendant.PhoneNumber,
                            Email=nonProfitDefendant.Email,
                        Address = nonProfitDefendant != null
                            ? new Address
                            {
                                CountryId = nonProfitDefendantAddress.CountryId,
                                CityId = nonProfitDefendantAddress.CityId,
                                CityText = nonProfitDefendantAddress.City,
                                District = nonProfitDefendantAddress.District,
                                Street = nonProfitDefendantAddress.Street,
                                BuildingNumber = nonProfitDefendantAddress.BuildingNumber,
                                PostCode = nonProfitDefendantAddress.PostCode,
                                AdditionalNumber = nonProfitDefendantAddress.AdditionalNumber,
                                ExtraInfo = nonProfitDefendantAddress.ExtraInfo
                            }
                            : null,
                        FoundingMember = FoundingMember != null
                            ? new FoundingMember
                            {
                                BirthDate = FoundingMember.BirthDate != null
                                    ? new BirthDate
                                    {
                                        Gregorian = FoundingMember.BirthDate.ToString("yyyy-MM-dd")
                                    }
                                    : null,
                                passporttNumber = FoundingMember.PassportNumber,
                                SaudiMobileNumber = FoundingMember.MobileNumber,
                                EmailAddress = FoundingMember.Email,
                                IdentityTypeId = FoundingMember.IdentityTypeId,
                                NationalId = FoundingMember.NationalID,
                                IqamaNumber = FoundingMember.IqamaNumber,
                                FirstName = FoundingMember.FirstName,
                                FatherName = FoundingMember.FatherName,
                                GrandfatherName = FoundingMember.GrandfatherName,
                                FamilyName = FoundingMember.FirstName,
                                NationalityId = FoundingMember.NationalityId,
                                BorderNumber = FoundingMember.BorderNumber,
                                VisaNumber = FoundingMember.VisaNumber
                            }
                            : null
                            
                    }
                    : null,
                 AgencyDefendant= AgencyDefendant.AgencyId != 0
                    ? new AgencyDefendant
                    {
                        AgencyId=AgencyDefendant.AgencyId
                    }
                    : null,
                   unRegisteredCompanyDefendant = unRegisteredCompanyDefendant.Headquarters!= null
                    ? new UnRegisteredCompanyDefendant
                    {
                        Name = unRegisteredCompanyDefendant.CompanyName,
                        Address = unRegisteredCompanyDefendantAddreess != null
                            ? new  Address
                            {
                                CountryId = unRegisteredCompanyDefendantAddreess.CountryId,
                                CityId = unRegisteredCompanyDefendantAddreess.CityId,
                                CityText = unRegisteredCompanyDefendantAddreess.City,
                                District = unRegisteredCompanyDefendantAddreess.District,
                                Street = unRegisteredCompanyDefendantAddreess.Street,
                                BuildingNumber = unRegisteredCompanyDefendantAddreess.BuildingNumber,
                                PostCode = unRegisteredCompanyDefendantAddreess.PostCode,
                                AdditionalNumber = unRegisteredCompanyDefendantAddreess.AdditionalNumber,
                                ExtraInfo = unRegisteredCompanyDefendantAddreess.ExtraInfo
                            }
                            : null,
                        PhoneNumber = unRegisteredCompanyDefendant.PhoneNumber,
                        Email = unRegisteredCompanyDefendant.Email,
                        representative = representative != null
                            ? new representative
                            {
                                BirthDate = representative.BirthDate != null
                                    ? new BirthDate
                                    {
                                        Gregorian = representative.BirthDate.ToString("yyyy-MM-dd")
                                    }
                                    : null,
                                passporttNumber = representative.PassportNumber,
                                SaudiMobileNumber = representative.MobileNumber,
                                EmailAddress = representative.Email,
                                IdentityTypeId = representative.IdentityTypeId,
                                NationalId = representative.NationalID,
                                IqamaNumber = representative.IqamaNumber,
                                FirstName = representative.FirstName,
                                FatherName = representative.FatherName,
                                GrandfatherName =representative.GrandfatherName,
                                FamilyName = representative.FamilyName,
                                NationalityId = representative.NationalityId,
                                BorderNumber = representative.BorderNumber,
                                VisaNumber = representative.VisaNumber
                            }
                            : null,

                        CrNumber=unRegisteredCompanyDefendant.CrNumber,
                        MISALicenseNo=unRegisteredCompanyDefendant.MISALicenseNo,
                        StartDate = unRegisteredCompanyDefendant.CrNumberStartDate != null
                            ? new BirthDate
                            {
                                Gregorian = unRegisteredCompanyDefendant.CrNumberStartDate.ToString("yyyy-MM-dd")
                            }
                            : null,
                        EndDate = unRegisteredCompanyDefendant.CrNumberEndDate != null
                            ? new BirthDate
                            {
                                Gregorian = unRegisteredCompanyDefendant.CrNumberEndDate.ToString("yyyy-MM-dd")
                            }
                            : null
                    }
                    : null,


            }
             


        }

                    ,
                    ExecutionClaim = new ExecutionClaim
                    {
                        Date = ExecutionClaim?.ExecutionClaimDate != null
                            ? new BirthDate
                            {
                                Gregorian = ExecutionClaim.ExecutionClaimDate.ToString("yyyy-MM-dd")
                            }
                            : null,
                        ResultId = ExecutionClaim?.ExecutionClaimResultId ?? 0,
                        RequestNumber = ExecutionClaim?.ExecutionClaimRequestNumber,
                        ExecutionClaimFile = new ExecutionClaimFile
                        {
                            FileName = ExecutionClaimFileName,
                            Base64Content = executionClaimFileBase64
                        },
                        ProofOfSubmitFile = new ExecutionClaimFile
                        {
                            FileName = ProofOfSubmitFileName,
                            Base64Content = proofOfSubmitFileBase64
                        }
                    },
                    Enforcement = new Enforcement
                    {
                        TypeId = Enforcement?.EnforcementTypeId ?? 0,
                        Number = Enforcement?.EnforcementNumber,
                        Date = Enforcement?.EnforcementDate != null
                            ? new BirthDate
                            {
                                Gregorian = Enforcement.EnforcementDate.ToString("yyyy-MM-dd")
                            }
                            : null,
                        Issuer = Enforcement?.EnforcementIssuer,
                        IssuePlace = Enforcement?.EnforcementIssuePlace,
                        HasFinancialClaims = Enforcement?.HasFinancialClaim ?? false,
                        Amount = Enforcement?.EnforcementAmount ?? 0,
                        AmountText = Enforcement?.EnforcementAmountLetters,
                        CurrencyId = 1,
                        PlaintiffIBANNumber = Enforcement?.PlaintiffIBAN,
                        Statement = Enforcement?.EnforcementStatement,
                        EnforcementItems = Enforcement?.EnforcementItems?
                            .Select(item => new EnforcementItem { Data = item })
                            .ToList(),
                        EnforcementFile = new EnforcementFile
                        {
                            FileName = EnforcementFileName,
                            Base64Content = enforcementFileBase64
                        },
                        EnforcementExtraFiles = enforcementExtraFilesBase64 != null && enforcementExtraFilesBase64.Any()
    ? enforcementExtraFilesBase64.Select(file => new EnforcementFile
    {
        FileName = !string.IsNullOrEmpty(GetFileNameFromBase64(file)) ? GetFileNameFromBase64(file) : "DefaultFileName.pdf",
        Base64Content = file
    }).ToList()
    : new List<EnforcementFile>()

            }
                };

                return requestModel;
            }


            var form = Request.Form;

            

            try
            {
                // Convert viewModel to RequestBodyModel as needed before calling the service
                var requestBodyModel = new RequestBodyModel
                {
                    // Populate with data from viewModel as needed
                };
                var req = CreateExampleRequest();
               //CreateGovAgencyRequestAsync method with the new parameters
                var response = await _govAgencyRequestService.CreateGovAgencyRequestAsync(
                   req
                );

                if (response.isSuccess && response.data != null)
                {
                    ViewBag.RequestNumber = response.data.RequestNumber;
                    _logger.LogInformation("Request {RequestNumber} processed successfully.", response.data.RequestNumber);
                    return RedirectToAction("Confirmation", new { response.data.RequestNumber });
                }
                else
                {

                    // Log the error
                    _logger.LogWarning("Service call failed: {ErrorMessage}", response.error);

                    // Redirect to the error action with the error message
                    return RedirectToAction("Error", new { errorMessage = response.error });
                   
                    //  ModelState.AddModelError(string.Empty, response.error?.FirstOrDefault()?.ErrorMessage ?? "An unknown error occurred.");
                   // return View(viewModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred while processing the request.");
                TempData["ErrorMessage"] = "An error occurred while processing your request. Please try again later.";
                return View(viewModel);
            }
        }

        private string GetFileNameFromBase64(string base64Content)
        {
            // Check if the base64 content follows the format "filename:base64data"
            int delimiterIndex = base64Content.IndexOf(':');

            if (delimiterIndex != -1)
            {
                // Extract the filename part before the delimiter
                return base64Content.Substring(0, delimiterIndex);
            }

            // If no delimiter is found, return a default filename or handle it based on your requirements
            return "DefaultFileName.pdf";
        }

        public IActionResult Error(string errorMessage)
        {
            ViewBag.ErrorMessage = errorMessage;
            return View();
        }

        private async Task<List<string>> ConvertFilesToBase64(IEnumerable<IFormFile> files)
        {
            var base64Files = new List<string>();

            foreach (var file in files)
            {
                string base64 = await ConvertFileToBase64(file);
                base64Files.Add(base64);
            }

            return base64Files;
        }

        private async Task<string> ConvertFileToBase64(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return string.Empty;

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            byte[] fileBytes = memoryStream.ToArray();
            return Convert.ToBase64String(fileBytes);
        }
       

           
       
        public async Task<ActionResult> GetDefendantTypePartialView(int defendantTypeId)
        {

            //  var countries = GetIndividualSelectList();
            //var xx=    GetCitesSelectList();
            var LicenseSources = await _lookupService.GetLicenseSourcesAsync(); //GetCitesSelectList();

            if (LicenseSources.Any())
            {
                // Extract the city names
                var licenseSourceItems = LicenseSources.Select(source => new SelectListItem
                {
                    Value = source.Id.ToString(),       // Adjust property names as needed
                    Text = source.nameAr                // Adjust property names as needed
                }).ToList();

                // Add an empty option at the beginning

                // Store the list in ViewData
                ViewData["LicenseSourceId"] = new SelectList(licenseSourceItems, "Value", "Text");
            }
            else
            {
                // Handle the error case if needed
                Console.WriteLine("API Error: Unable to retrieve city data");
                ViewData["LicenseSourceId"] = new SelectList(new List<string>()); // Empty list or handle the error accordingly
            }
            var Cites = await _lookupService.GetCitiesAsync(); //GetCitesSelectList();

            if (Cites.Any())
            {
                // Extract the city names
                var cityNames = Cites.Select(city =>new SelectListItem
                {
                    Value = city.id.ToString(),    // Adjust property names as needed
                    Text = city.nameAr             // Adjust property names as needed
                }).ToList();

                // Store the city names in ViewData or pass it to the view model, depending on your preference
                ViewData["CityId"] = new SelectList(cityNames, "Value", "Text");
            }
            else
            {
                // Handle the error case if needed
                Console.WriteLine("API Error: Unable to retrieve city data");
                ViewData["CityId"] = new SelectList(new List<string>()); // Empty list or handle the error accordingly
            }
            var countries = await _lookupService.GetCountriesAsync(); //GetCitesSelectList();

            if (countries.Any())
            {
                // Extract the city names
                var countryNames = countries.Select(country  => new SelectListItem
                {
                    Value = country.Id.ToString(),    // Adjust property names as needed
                    Text = country.nameAr             // Adjust property names as needed
                }).ToList();


                // Store the city names in ViewData or pass it to the view model, depending on your preference
                ViewData["CountryId"] = new SelectList(countryNames, "Value", "Text");
            }
            else
            {
                // Handle the error case if needed
                Console.WriteLine("API Error: Unable to retrieve city data");
                ViewData["CountryId"] = new SelectList(new List<string>()); // Empty list or handle the error accordingly
            }
            //ViewData["CountryId"] = countries;
            //ViewData["IdentityTypeId"] = GetCitesSelectList(); 
            //ViewData["LicenseSources"] = GetCitesSelectList();

            var IdentityTypes = await _lookupService.GetIdentityTypesAsync(); //GetCitesSelectList();

            if (IdentityTypes.Any())
            {
                // Extract the city names
                var IdentityTypenames = IdentityTypes.Select(IdentityType =>  new SelectListItem
                {
                    Value = IdentityType.Id.ToString(),    // Adjust property names as needed
                    Text = IdentityType.nameAr             // Adjust property names as needed
                }).ToList();

                // Store the city names in ViewData or pass it to the view model, depending on your preference
                ViewData["IdentityTypeId"] = new SelectList(IdentityTypenames, "Value", "Text");
            }
            else
            {
                // Handle the error case if needed
                Console.WriteLine("API Error: Unable to retrieve city data");
                ViewData["IdentityTypeId"] = new SelectList(new List<string>()); // Empty list or handle the error accordingly
            }



            // Now put the country list into ViewData or the ViewBag before returning the partial view
            //ViewData["CityId"] = Cites;
            // Based on defendantTypeId, return the corresponding partial view
            switch (defendantTypeId)
            {
                case 1:


                    return PartialView("RegisteredCompanyInfo", new RegisteredCompanyInfo());
                case 2:
                    return PartialView("UnregisteredCompanyInfo", new UnregisteredCompanyInfo());
                case 3:
                    return PartialView("EndowmentInfo", new EndowmentInfo());
                case 4:
                    return PartialView("CharitableAssociationInfo", new CharitableAssociationInfo());
                case 5:
                    return PartialView("CivilOrganizationInfo", new CivilOrganizationInfo());
               
                case 6:


                    return PartialView("IndividualInfoType", new IndividualInfoType());
                case 7:

                    return RedirectToAction(nameof(AdministrativeAgencyInformation));


                // ... other cases ...
                default:
                    return Content("Select a valid defendant type.");
            }
        }


        private bool ShouldIncludeSubModel(object subModel)
        {
            return subModel != null;
        }
        public IActionResult Confirmation(long? requestNumber)
        {
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            }

            ViewBag.RequestNumber = requestNumber;
            return View();
        }
    }
}
