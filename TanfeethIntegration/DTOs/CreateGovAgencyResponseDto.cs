using Newtonsoft.Json;

namespace TanfeethIntegration.DTOs
{

    public class CreateGovAgencyResponseDto
    {
        public ApiResponseData Data { get; set; }
        public bool IsSuccess => Data?.IsSuccess ?? false;
        public long? RequestNumber => Data?.RequestNumber;
        public string Message => Data?.Message;
        public ApiErrorDetails Error { get; set; }
    }
  
    public class ApiResponseData
    {
        [JsonProperty("requestNumber")]
        public long? RequestNumber { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class ApiErrorDetails
    {
        public string Path { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public string ReferenceId { get; set; }
        public Dictionary<string, object> Params { get; set; }
    }




}
