using System.Net;
using System.Text.Json.Serialization;

namespace ECommerce.DTO
{
   [Serializable]
    public class ApiResponseDto
    {

        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public bool Success { get; set; }
        public object? Result { get; set; }
        public List<ApiMessage> Messages { get; set; } = [];
        public string? Title { get; set; }

    }
    [Serializable]
    public class ApiMessage
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("format")]
        public string MessageFormat { get; set; } = string.Empty;

        [JsonPropertyName("msg")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("params")]
        public object[] Parameters { get; set; } = [];
    }
 
}
