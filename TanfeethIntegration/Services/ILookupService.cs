using TanfeethIntegration.DTOs;
using TanfeethIntegration.Models;

namespace TanfeethIntegration.Services
{
    public interface ILookupService
    {
        Task<IEnumerable<AgencyLookupDto>> GetAgenciesAsync();
        Task<IEnumerable<DefendantTypeLookupDto>> GetDefendantTypesAsync();
        Task<IEnumerable<LicenseSourceLookupDto>> GetLicenseSourcesAsync();
        Task<IEnumerable<CountryLookupDto>> GetCountriesAsync();
        Task<IEnumerable<CityLookupDto>> GetCitiesAsync();
        Task<IEnumerable<IdentityTypeLookupDto>> GetIdentityTypesAsync();
        Task<IEnumerable<ExecutionClaimResultLookupDto>> GetExecutionClaimResultsAsync();
        Task<IEnumerable<EnforcementTypeLookupDto>> GetEnforcementTypesAsync();
        Task<IEnumerable<RequestStatusDTO>> GetExecutionRequestStatusAsync();
        
        Task<IEnumerable<CourtDataItem>> GetCourtsAsync();
    }
}
