using Newtonsoft.Json;

namespace BTCGatewayAPI.Bitcoin.Models
{
    public class TxOutput : TXInfo
    {
        [JsonProperty("scriptPubKey", Order = 2)]
        public string ScriptPubKey { get; set; }

        [JsonProperty("redeemScript", Order = 3)]
        public string RedeemScript { get; set; }

        [JsonProperty("amount", Order = 4)]
        public decimal Amount { get; set; }
    }
}
