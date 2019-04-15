using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Models
{
    [DataContract]
    public class TxOutput : TXInfo
    {
        [DataMember(Name = "scriptPubKey", Order = 2)]
        public string ScriptPubKey { get; set; }

        [DataMember(Name = "redeemScript", Order = 3)]
        public string RedeemScript { get; set; }

        [DataMember(Name = "amount", Order = 4)]
        public decimal Amount { get; set; }
    }
}
