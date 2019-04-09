using Newtonsoft.Json;

namespace BTCGatewayAPI.Bitcoin.Models
{
    public class Unspent : TxOutput
    {
        [JsonProperty("address", Order = 2)]
        public string Address { get; set; }

        [JsonProperty("label", Order = 3)]
        public string Label { get; set; }

        [JsonProperty("confirmations", Order = 7)]
        public int Confirmations { get; set; }

        [JsonProperty("spendable", Order = 8)]
        public bool Spendable { get; set; }

        [JsonProperty("solvable", Order = 9)]
        public bool Solvable { get; set; }

        [JsonProperty("safe", Order = 10)]
        public bool Safe { get; set; }
    }
}
