namespace TanfeethIntegration.Models
{
    public class ApiResponse<T>
    {
        //public bool IsSuccess { get; set; }
        //public T Data { get; set; }
        //public List<ApiError> Errors { get; set; } = new List<ApiError>();
        //public int? Code { get; set; }
        //public string Message { get; set; }
        public T data { get; set; }
        public bool isSuccess { get; set; }
        public string error { get; set; }
    }


    public class ApiError
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string ArabicDescription { get; set; }

        private static readonly Dictionary<int, string[]> ErrorCodeDescriptions = new Dictionary<int, string[]>
    {
        {1001, new[] { "Unauthorized", "غير مصرح" }},
        {1002, new[] { "Field '{0}' input is invalid", "إدخال الحقل '{0}' غير صالح" }},
        {1003, new[] { "Field '{0}' length is invalid", "طول الحقل '{0}' غير صالح" }},
        // Add other error codes and descriptions as necessary.
        {2001, new[] { "Internal Service Error", "خطأ داخلي في الخدمة" }},
        {2002, new[] { "Service is Unavailable", "الخدمة غير متوفرة" }}
    };

        public ApiError(int code)
        {
            ErrorCode = code;
            if (ErrorCodeDescriptions.TryGetValue(code, out var descriptions))
            {
                ErrorMessage = descriptions[0];
                ArabicDescription = descriptions[1];
            }
            else
            {
                ErrorMessage = "An unknown error occurred.";
                ArabicDescription = "حدث خطأ غير معروف.";
            }
        }

        public ApiError(int code, string customMessage, string customArabicDescription)
        {
            ErrorCode = code;
            ErrorMessage = customMessage;
            ArabicDescription = customArabicDescription;
        }

        public static ApiError CreateFormattedError(int code, params object[] args)
        {
            if (ErrorCodeDescriptions.TryGetValue(code, out var descriptions))
            {
                // Use string.Format to insert the parameters into the error message, if needed.
                var formattedMessage = args.Length > 0 ? string.Format(descriptions[0], args) : descriptions[0];
                var formattedArabicMessage = args.Length > 0 ? string.Format(descriptions[1], args) : descriptions[1];

                return new ApiError(code, formattedMessage, formattedArabicMessage);
            }

            // If the error code does not exist in the dictionary, return the default error.
            return new ApiError(code);
        }
    }
}
