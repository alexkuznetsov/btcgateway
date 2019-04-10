using Newtonsoft.Json;

namespace BTCGatewayAPI.Bitcoin.Models
{
    public class Transaction
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("vout")]
        public int Vout { get; set; }

        [JsonProperty("fee")]
        public decimal Fee { get; set; }

        [JsonProperty("confirmations")]
        public int Confirmations { get; set; }

        [JsonProperty("trusted")]
        public bool Trusted { get; set; }

        [JsonProperty("blockhash")]
        public string Blockhash { get; set; }

        [JsonProperty("blockindex")]
        public int Blockindex { get; set; }

        [JsonProperty("blocktime")]
        public int Blocktime { get; set; }

        [JsonProperty("txid")]
        public string Txid { get; set; }

        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("timereceived")]
        public int Timereceived { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("bip125-replaceable")]
        public string BIP125Replaceable { get; set; }

        [JsonProperty("abandoned")]
        public bool Abandoned { get; set; }

        public bool IsReceive() => string.Compare("receive", Category) == 0;

        public bool IsSend() => string.Compare("send", Category) == 0;
    }
}
