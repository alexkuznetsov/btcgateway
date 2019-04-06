using Newtonsoft.Json;

namespace BTCGatewayAPI.Bitcoin.Models
{
    public class FundRecivier
    {
        [JsonProperty("address", Order = 0)]
        public string Address { get; set; }

        [JsonProperty("amount", Order = 0)]
        public decimal Amount { get; set; }
    }
}
