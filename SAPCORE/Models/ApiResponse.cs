using System.Text.Json.Serialization;

namespace SAPCORE.Models
{
    public class Message
    {
        [JsonPropertyName("MessageType")]
        public string MessageType { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("Business Partner Number")]
        public string BusinessPartnerNumber { get; set; }

        [JsonPropertyName("Document Type")]
        public string DocumentType { get; set; }
    }

    public class ApiResponse
    {
        [JsonPropertyName("Message")]
        public Message Message { get; set; }
    }
}
