using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Models
{
    [DataContract]
    public class EstimateSmartFee
    {
        [DataMember(Name = "feerate", Order = 0)]
        public decimal Feerate { get; set; }

        [DataMember(Name = "blocks", Order = 1)]
        public int Blocks { get; set; }
    }
}
