using Newtonsoft.Json;

namespace TanfeethIntegration.DTOs
{
    public class GetRequestStatusResponseDto
    {
        public ResponseData Data { get; set; }
        public bool IsSuccess { get; set; }
        public long? requestNumber { get; set; }
        public ApiErrorDetails Error { get; set; }

    }

    public class ResponseData
    {
        public int requestNumber { get; set; }
        public int statusId { get; set; }
        public bool enablePayment { get; set; }

        // Add other properties as needed to represent the response data

        public List<ValidationResultDto> ValidationResults { get; set; }
    }

    public class ValidationResultDto
    {
        public string code { get; set; }
        public string details { get; set; }
        public string path { get; set; }
    }
}
