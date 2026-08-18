namespace TanfeethIntegration.DTOs
{
    public class CityLookupResponse
    {
        public IEnumerable<CityLookupDto> Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
    }

}
