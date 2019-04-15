using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Models
{
    [DataContract]
    public class TXInfo
    {
        [DataMember(Name = "txid", Order = 0)]
        public string Txid { get; set; }

        [DataMember(Name = "vout", Order = 1)]
        public int Vout { get; set; }
    }
}
