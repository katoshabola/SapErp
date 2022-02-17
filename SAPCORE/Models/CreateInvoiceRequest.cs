using System.Text.Json.Serialization;

namespace SAPCORE.Models
{
    public class InvoiceHeader
    {
        [JsonPropertyName("DocDate")]
        public string DocDate { get; set; }

        [JsonPropertyName("PostingDate")]
        public string PostingDate { get; set; }

        [JsonPropertyName("DocDueDate")]
        public string DocDueDate { get; set; }

        [JsonPropertyName("RequiredDate")]
        public string RequiredDate { get; set; }

        [JsonPropertyName("ValidUntil")]
        public string ValidUntil { get; set; }

        [JsonPropertyName("ObjectType")]
        public string ObjectType { get; set; }

        [JsonPropertyName("DocCurrency")]
        public string DocCurrency { get; set; }

        [JsonPropertyName("CardCode")]
        public string CardCode { get; set; }

        [JsonPropertyName("CardName")]
        public string CardName { get; set; }

        [JsonPropertyName("InvoiceType")]
        public string InvoiceType { get; set; }

        [JsonPropertyName("SourceNumber")]
        public string SourceNumber { get; set; }

        [JsonPropertyName("Action")]
        public string Action { get; set; }

        [JsonPropertyName("Rounding")]
        public string Rounding { get; set; }

        [JsonPropertyName("Reference")]
        public string Reference { get; set; }

        [JsonPropertyName("Remarks")]
        public string Remarks { get; set; }
        public string SalesPersonCode { get; internal set; }
        public string DocumentOwnerCode { get; internal set; }
        public string DocType { get; internal set; }
    }

    public class ItemRow
    {
        [JsonPropertyName("ItemCode")]
        public string ItemCode { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("Quantity")]
        public string Quantity { get; set; }

        [JsonPropertyName("AcctCode")]
        public string AcctCode { get; set; }

        [JsonPropertyName("UnitPrice")]
        public string UnitPrice { get; set; }

        [JsonPropertyName("VatGroup")]
        public string VatGroup { get; set; }

        [JsonPropertyName("LineTotal")]
        public string LineTotal { get; set; }

        [JsonPropertyName("WarehouseCode")]
        public string WarehouseCode { get; set; }
    }

    public class CreateInvoiceRequest
    {
        [JsonPropertyName("Header")]
        public InvoiceHeader Header { get; set; }

        [JsonPropertyName("Rows")]
        public List<ItemRow> Rows { get; set; }
    }
}
