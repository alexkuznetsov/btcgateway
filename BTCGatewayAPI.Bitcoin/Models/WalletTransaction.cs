using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Models
{
    public class WalletTransaction
    {
        [DataMember(Name = "amount")]
        public decimal Amount { get; set; }

        [DataMember(Name = "confirmations")]
        public int Confirmations { get; set; }

        [DataMember(Name = "blockhash")]
        public string Blockhash { get; set; }

        [DataMember(Name = "blockindex")]
        public int Blockindex { get; set; }

        [DataMember(Name = "blocktime")]
        public int Blocktime { get; set; }

        [DataMember(Name = "txid")]
        public string Txid { get; set; }

        [DataMember(Name = "walletconflicts")]
        public string[] Walletconflicts { get; set; }

        [DataMember(Name = "time")]
        public int Time { get; set; }

        [DataMember(Name = "timereceived")]
        public int Timereceived { get; set; }

        [DataMember(Name = "bip125-replaceable")]
        public string Bip125Replaceable { get; set; }

        [DataMember(Name = "details")]
        public WalletTransactionDetails[] Details { get; set; }

        [DataMember(Name = "hex")]
        public string Hex { get; set; }
    }

    public class WalletTransactionDetails
    {
        [DataMember(Name = "address")]
        public string Address { get; set; }

        [DataMember(Name = "category")]
        public string Category { get; set; }

        [DataMember(Name = "amount")]
        public decimal Amount { get; set; }

        [DataMember(Name = "label")]
        public string Label { get; set; }

        [DataMember(Name = "vout")]
        public int VOut { get; set; }
    }
}
