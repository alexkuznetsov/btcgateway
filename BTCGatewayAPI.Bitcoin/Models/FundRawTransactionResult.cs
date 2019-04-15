using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Models
{
    public class FundRawTransactionResult
    {
        [DataMember(Name = "hex")]
        public string Hex { get; set; }

        [DataMember(Name = "fee")]
        public decimal Fee { get; set; }

        [DataMember(Name = "changepos")]
        public int ChangePos { get; set; }
    }
}
