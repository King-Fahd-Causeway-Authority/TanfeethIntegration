using Newtonsoft.Json;
using TanfeethIntegration.DTOs;
using TanfeethIntegration.Models;

namespace TanfeethIntegration.Services
{
    public class LookupService : ILookupService
    {
        private readonly HttpClient _httpClient;


        private readonly string _baseApiUrl;

        public LookupService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();

            // Get the base URL and API Keys from the configuration
            _baseApiUrl = configuration.GetValue<string>("ExternalService:BaseApiUrl");
            var apiKey = configuration.GetValue<string>("ExternalService:x-Gateway-APIKey");
            var appKey = configuration.GetValue<string>("ExternalService:App-Key");

            // Configure the HttpClient with the necessary headers
            _httpClient.DefaultRequestHeaders.Clear(); // Clear existing headers
            _httpClient.DefaultRequestHeaders.Add("x-Gateway-APIKey", apiKey);
            _httpClient.DefaultRequestHeaders.Add("App-Key", appKey);
        }

        public async Task<IEnumerable<CourtDataItem>> GetCourtsAsync()
        {
            return await GetLookupAsync<CourtDataItem>("Courts");
        }
        // Following the example method to get the agencies, 
        // we can replicate similar methods for each lookup type:

        public async Task<IEnumerable<DefendantTypeLookupDto>> GetDefendantTypesAsync()
        {
            return await GetLookupAsync<DefendantTypeLookupDto>("DefendantTypes");
        }
        public async Task<IEnumerable<AgencyLookupDto>> GetAgenciesAsync()
        {
            return await GetLookupAsync<AgencyLookupDto>("Agencies");
        }

        public async Task<IEnumerable<LicenseSourceLookupDto>> GetLicenseSourcesAsync()
        {
            return await GetLookupAsync<LicenseSourceLookupDto>("LicenseSources");
        }

        public async Task<IEnumerable<CountryLookupDto>> GetCountriesAsync()
        {
            return await GetLookupAsync<CountryLookupDto>("Countries");
        }

        public async Task<IEnumerable<RequestStatusDTO>> GetExecutionRequestStatusAsync()
        {
            return await GetLookupAsync<RequestStatusDTO>("ExecutionRequestStatus");
        }

        public async Task<IEnumerable<CityLookupDto>> GetCitiesAsync()
        {
            // Call GetLookupAsync without specifying IEnumerable<>
            return await GetLookupAsync<CityLookupDto>("Cities");
        }



        public async Task<IEnumerable<IdentityTypeLookupDto>> GetIdentityTypesAsync()
        {
            return await GetLookupAsync<IdentityTypeLookupDto>("IdentityTypes");
        }

        public async Task<IEnumerable<ExecutionClaimResultLookupDto>> GetExecutionClaimResultsAsync()
        {
            return await GetLookupAsync<ExecutionClaimResultLookupDto>("ExecutionClaimResults");
        }

        public async Task<IEnumerable<EnforcementTypeLookupDto>> GetEnforcementTypesAsync()
        {
            return await GetLookupAsync<EnforcementTypeLookupDto>("EnforcementTypes");
        }

        // The GetLookupAsync method uses the generic type T to deserialize the JSON content to
        // the corresponding type of lookup DTO.
        private async Task<IEnumerable<T>> GetLookupAsync<T>(string endpoint)
        {
            string url = $"{_baseApiUrl}/Lookup/{endpoint}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                try
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<IEnumerable<T>>>(jsonResponse);

                    // Check if the API call was successful
                    if (apiResponse.isSuccess)
                    {
                        // Return the data property
                        return apiResponse.data;
                    }
                    else
                    {
                        // Handle the error case if needed
                        Console.WriteLine($"API Error: {apiResponse.error}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle deserialization exception
                    Console.WriteLine($"Error deserializing response: {ex.Message}");
                }
            }
            else
            {
                // Log the failure status code
                Console.WriteLine($"Service call failed with status code: {response.StatusCode}");
            }

            // If the API call was not successful or encountered an issue during deserialization, handle it accordingly.
            return Enumerable.Empty<T>();
        }



    }
}
