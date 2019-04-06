using Newtonsoft.Json;

namespace BTCGatewayAPI.Bitcoin.Models
{
    public class FundRawTransactionResult
    {
        [JsonProperty("hex")]
        public string Hex { get; set; }

        [JsonProperty("fee")]
        public decimal Fee { get; set; }

        [JsonProperty("changepos")]
        public int ChangePos { get; set; }
    }
}
