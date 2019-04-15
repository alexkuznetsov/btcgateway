using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Models
{
    public class Transaction
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
        public int Vout { get; set; }

        [DataMember(Name = "fee")]
        public decimal Fee { get; set; }

        [DataMember(Name = "confirmations")]
        public int Confirmations { get; set; }

        [DataMember(Name = "trusted")]
        public bool Trusted { get; set; }

        [DataMember(Name = "blockhash")]
        public string Blockhash { get; set; }

        [DataMember(Name = "blockindex")]
        public int Blockindex { get; set; }

        [DataMember(Name = "blocktime")]
        public int Blocktime { get; set; }

        [DataMember(Name = "txid")]
        public string Txid { get; set; }

        [DataMember(Name = "time")]
        public int Time { get; set; }

        [DataMember(Name = "timereceived")]
        public int Timereceived { get; set; }

        [DataMember(Name = "comment")]
        public string Comment { get; set; }

        [DataMember(Name = "bip125-replaceable")]
        public string BIP125Replaceable { get; set; }

        [DataMember(Name = "abandoned")]
        public bool Abandoned { get; set; }

        public bool IsReceive() => string.Compare("receive", Category) == 0;

        public bool IsSend() => string.Compare("send", Category) == 0;
    }
}
