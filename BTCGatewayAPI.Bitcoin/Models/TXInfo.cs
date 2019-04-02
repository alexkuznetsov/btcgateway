using Newtonsoft.Json;

namespace BTCGatewayAPI.Bitcoin.Models
{
    public class TXInfo
    {
        [JsonProperty("txid", Order = 0)]
        public string Txid { get; set; }

        [JsonProperty("vout", Order = 1)]
        public int Vout { get; set; }
    }
}
