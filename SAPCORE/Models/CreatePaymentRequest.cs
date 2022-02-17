using System.Text.Json.Serialization;

namespace SAPCORE.Models
{
    public class PaymentHeader
    {
        [JsonPropertyName("SourceNumber")]
        public string SourceNumber { get; set; }

        [JsonPropertyName("DocDate")]
        public string DocDate { get; set; }

        [JsonPropertyName("PostingDate")]
        public string PostingDate { get; set; }

        [JsonPropertyName("Type")]
        public string Type { get; set; }

        [JsonPropertyName("DocCurrency")]
        public string DocCurrency { get; set; }

        [JsonPropertyName("CardType")]
        public string CardType { get; set; }

        [JsonPropertyName("CardCode")]
        public string CardCode { get; set; }

        [JsonPropertyName("Action")]
        public string Action { get; set; }

        [JsonPropertyName("Rounding")]
        public string Rounding { get; set; }

        [JsonPropertyName("InvoiceDocEntry")]
        public string InvoiceDocEntry { get; set; }

        [JsonPropertyName("ReceiptNo")]
        public string ReceiptNo { get; set; }
    }

    public class Payment
    {
        [JsonPropertyName("PaymentDate")]
        public string PaymentDate { get; set; }

        [JsonPropertyName("PaymentReference")]
        public string PaymentReference { get; set; }

        [JsonPropertyName("CardValidUntil")]
        public string CardValidUntil { get; set; }

        [JsonPropertyName("PaymentType")]
        public string PaymentType { get; set; }

        [JsonPropertyName("Account")]
        public string Account { get; set; }

        [JsonPropertyName("Amount")]
        public string Amount { get; set; }
    }

    public class CreatePaymentRequest
    {
        [JsonPropertyName("Header")]
        public PaymentHeader Header { get; set; }

        [JsonPropertyName("Payments")]
        public List<Payment> Payments { get; set; }
    }
}
