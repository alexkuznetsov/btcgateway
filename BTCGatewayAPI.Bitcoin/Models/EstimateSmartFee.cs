using Newtonsoft.Json;

namespace BTCGatewayAPI.Bitcoin.Models
{
    public class EstimateSmartFee
    {
        [JsonProperty("feerate", Order = 0)]
        public decimal Feerate { get; set; }

        [JsonProperty("blocks", Order = 1)]
        public int Blocks { get; set; }
    }
}
