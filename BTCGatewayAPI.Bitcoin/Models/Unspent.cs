using Newtonsoft.Json;

namespace BTCGatewayAPI.Bitcoin.Models
{
    public class Unspent : TxOutput
    {
        //[JsonProperty("txid", Order = 0)]
        //public string Txid { get; set; }

        //[JsonProperty("vout", Order = 1)]
        //public int Vout { get; set; }

        [JsonProperty("address", Order = 2)]
        public string Address { get; set; }

        [JsonProperty("label", Order = 3)]
        public string Label { get; set; }

        //[JsonProperty("redeemScript", Order = 4)]
        //public string RedeemScript { get; set; }

        //[JsonProperty("scriptPubKey", Order = 5)]
        //public string ScriptPubKey { get; set; }

        [JsonProperty("amount", Order = 6)]
        public decimal Amount { get; set; }

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
