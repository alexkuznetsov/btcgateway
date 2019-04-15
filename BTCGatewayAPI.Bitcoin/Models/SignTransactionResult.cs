using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Models
{
    public class SignTransactionResult
    {
        [DataMember(Name = "hex")]
        public string Hex { get; set; }

        [DataMember(Name = "complete")]
        public bool Complete { get; set; }
    }
}
