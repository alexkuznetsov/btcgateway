using Newtonsoft.Json;

namespace BTCGatewayAPI.Bitcoin.Models
{
    public class SignTransactionResult
    {
        [JsonProperty("hex")]
        public string Hex { get; set; }

        [JsonProperty("complete")]
        public bool Complete { get; set; }
    }
}
