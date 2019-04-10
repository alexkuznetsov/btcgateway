using Newtonsoft.Json;

namespace BTCGatewayAPI.Bitcoin.Models
{
    public class WalletTransaction
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("confirmations")]
        public int Confirmations { get; set; }

        [JsonProperty("blockhash")]
        public string Blockhash { get; set; }

        [JsonProperty("blockindex")]
        public int Blockindex { get; set; }

        [JsonProperty("blocktime")]
        public int Blocktime { get; set; }

        [JsonProperty("txid")]
        public string Txid { get; set; }

        [JsonProperty("walletconflicts")]
        public string[] Walletconflicts { get; set; }

        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("timereceived")]
        public int Timereceived { get; set; }

        [JsonProperty("bip125-replaceable")]
        public string Bip125Replaceable { get; set; }

        [JsonProperty("details")]
        public WalletTransactionDetails[] Details { get; set; }

        [JsonProperty("hex")]
        public string Hex { get; set; }
    }

    public class WalletTransactionDetails
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
        public int VOut { get; set; }
    }
}
