using System.Text.Json.Serialization;

namespace SAPCORE.Models
{

    public class BPInformation
    {
        [JsonPropertyName("CardCode")]
        public string CardCode { get; set; }

        [JsonPropertyName("CardName")]
        public string CardName { get; set; }

        [JsonPropertyName("CardType")]
        public string CardType { get; set; }

        [JsonPropertyName("GroupCode")]
        public string GroupCode { get; set; }

        [JsonPropertyName("Telephone1")]
        public string Telephone1 { get; set; }

        [JsonPropertyName("Telephone2")]
        public string Telephone2 { get; set; }

        [JsonPropertyName("MobilePhone")]
        public string MobilePhone { get; set; }

        [JsonPropertyName("KRAPIN")]
        public string KRAPIN { get; set; }

        [JsonPropertyName("Email")]
        public string Email { get; set; }

        [JsonPropertyName("Fax")]
        public string Fax { get; set; }

        [JsonPropertyName("Action")]
        public string Action { get; set; }

        [JsonPropertyName("AlternateCardCode")]
        public string AlternateCardCode { get; set; }

        [JsonPropertyName("Uniqueid")]
        public string Uniqueid { get; set; }

        [JsonPropertyName("CreateDateTime")]
        public string CreateDateTime { get; set; }
    }

    public class BilltoAdress
    {
        [JsonPropertyName("AddressName1")]
        public string AddressName1 { get; set; }

        [JsonPropertyName("AddressName2")]
        public string AddressName2 { get; set; }

        [JsonPropertyName("POBox")]
        public int POBox { get; set; }

        [JsonPropertyName("Code")]
        public string Code { get; set; }

        [JsonPropertyName("City")]
        public string City { get; set; }
    }

    public class ShiptoAdress
    {
        [JsonPropertyName("AddressName1")]
        public string AddressName1 { get; set; }

        [JsonPropertyName("AddressName2")]
        public string AddressName2 { get; set; }

        [JsonPropertyName("POBox")]
        public int POBox { get; set; }

        [JsonPropertyName("Code")]
        public string Code { get; set; }

        [JsonPropertyName("City")]
        public string City { get; set; }
    }

    public class CreateBusinessPartnerRequest
    {
        [JsonPropertyName("BPInformation")]
        public BPInformation BPInformation { get; set; }

        [JsonPropertyName("BilltoAdress")]
        public BilltoAdress BilltoAdress { get; set; }

        [JsonPropertyName("ShiptoAdress")]
        public ShiptoAdress ShiptoAdress { get; set; }
    }
}
