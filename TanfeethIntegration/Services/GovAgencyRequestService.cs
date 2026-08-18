using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Text;
using TanfeethIntegration.Data;
using TanfeethIntegration.DTOs;
using TanfeethIntegration.Models;

namespace TanfeethIntegration.Services
{
    public class GovAgencyRequestService : IGovAgencyRequestService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey; // The X-API-Key
        private readonly string _appKey; // The APP-Key
        private readonly string _baseApiUrl;
        private readonly IConfiguration _configuration;
        private readonly LogDbContext _logDbContext;  // Add this linez



        public GovAgencyRequestService(IHttpClientFactory httpClientFactory, IConfiguration configuration, LogDbContext logDbContext)
        {
            _httpClient = httpClientFactory.CreateClient();
            _baseApiUrl = configuration.GetValue<string>("ExternalService:BaseApiUrl");
            _apiKey = configuration.GetValue<string>("ExternalService:x-Gateway-APIKey");
            _appKey = configuration.GetValue<string>("ExternalService:App-Key"); // New APP-Key
            _configuration = configuration;
            _logDbContext = logDbContext;
        }

        

        public async Task<ApiResponse<CreateGovAgencyResponseDto>> CreateGovAgencyRequestAsync(
     RequestModel requestModel)
        {
            string url = $"{_baseApiUrl}/ExecutionRequest/CreateGovAgencyRequest";

            // Use the new header for the request along with the API key and APP key
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("x-Gateway-APIKey", _apiKey);
            _httpClient.DefaultRequestHeaders.Add("App-Key", _appKey);

            // Add authorization key to your request headers. Omit if not required
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

           

            //RequestModel request = CreateExampleRequest();

            // Serialize the RequestModel instance to JSON
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented // Optionally, you can set formatting as needed
            };

            // Serialize the object to JSON with camelCase property names
            string requestJson = JsonConvert.SerializeObject(requestModel, jsonSerializerSettings);           // var requestJson = JsonConvert.SerializeObject(requestDto);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            // Log the serialized request
            //await LogRequestResponseAsync(requestJson, null, 0);

            // Send the POST request
            var response = await _httpClient.PostAsync(url, content);

            // Initialize the ApiResponse
            var apiResponse = new ApiResponse<CreateGovAgencyResponseDto>
            {
                isSuccess = response.IsSuccessStatusCode
            };

            // Process the response
            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var resultDto = JsonConvert.DeserializeObject<CreateGovAgencyResponseDto>(responseContent);
                apiResponse.data = resultDto;
            }
            else
            {
                var errorDto = JsonConvert.DeserializeObject<CreateGovAgencyResponseDto>(responseContent);
                apiResponse.error = errorDto?.Error?.Message;
            }

            // Log the response
             await LogRequestResponseAsync(requestJson, responseContent, (int)response.StatusCode);

            return apiResponse;
        }

        public async Task<ApiResponse<GetRequestStatusResponseDto>> GetRequestStatusAsync(int requestNumber)
        {
            string url = $"{_baseApiUrl}/ExecutionRequest/GetRequestStatus";

            // Use the new header for the request along with the API key and APP key
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("x-Gateway-APIKey", _apiKey);
            _httpClient.DefaultRequestHeaders.Add("App-Key", _appKey);

            // Create a model for the request
            var getRequestStatusRequest = new GetRequestStatusResponseDto
            {
               requestNumber = requestNumber
            };

            // Serialize the GetRequestStatusRequestDto instance to JSON
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };

            // Serialize the object to JSON with camelCase property names
            string requestJson = JsonConvert.SerializeObject(getRequestStatusRequest, jsonSerializerSettings);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            // Log the serialized request
            //await LogRequestResponseAsync(requestJson, null, 0);

            // Send the POST request
            var response = await _httpClient.PostAsync(url, content);

            // Initialize the ApiResponse
            var apiResponse = new ApiResponse<GetRequestStatusResponseDto>
            {
                isSuccess = response.IsSuccessStatusCode
            };

            // Process the response
            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var resultDto = JsonConvert.DeserializeObject<GetRequestStatusResponseDto>(responseContent);
                apiResponse.data = resultDto;
            }
            else
            {
                var errorDto = JsonConvert.DeserializeObject<ApiErrorDetails>(responseContent);
                apiResponse.error = errorDto?.Message;
            }

            // Log the response
            await LogRequestResponseAsync(requestJson, responseContent, (int)response.StatusCode);

            return apiResponse;
        }


        private async Task LogRequestResponseAsync(string request, string response, int statusCode)
        {
            try
            {
                var logEntry = new RequestResponseLog
                {
                    Request = request,
                    Response = response,
                    ResponseStatusCode = statusCode,
                    Timestamp = DateTime.UtcNow
                };

                _logDbContext.RequestResponseLogs.Add(logEntry);
                await _logDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error logging request/response: {ex.Message}");
                // You can also log the exception to a dedicated logging service
                // log.LogError($"Error logging request/response: {ex.Message}", ex);
            }
        }


    }
}
