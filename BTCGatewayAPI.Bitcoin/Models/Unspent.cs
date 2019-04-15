using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Models
{
    public class Unspent : TxOutput
    {
        [DataMember(Name = "address", Order = 2)]
        public string Address { get; set; }

        [DataMember(Name = "label", Order = 3)]
        public string Label { get; set; }

        [DataMember(Name = "confirmations", Order = 7)]
        public int Confirmations { get; set; }

        [DataMember(Name = "spendable", Order = 8)]
        public bool Spendable { get; set; }

        [DataMember(Name = "solvable", Order = 9)]
        public bool Solvable { get; set; }

        [DataMember(Name = "safe", Order = 10)]
        public bool Safe { get; set; }
    }
}
