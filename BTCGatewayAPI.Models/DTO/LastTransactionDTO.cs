using System;
using System.Runtime.Serialization;

namespace BTCGatewayAPI.Models.DTO
{
    [DataContract]
    public class LastTransactionDTO
    {
        [DataMember(Name = "date")]
        public DateTime Date { get; set; }

        [DataMember(Name = "address")]
        public string Address { get; set; }

        [DataMember(Name = "amount")]
        public decimal Amount { get; set; }

        [DataMember(Name = "confirmation")]
        public int Confirmation { get; set; }
    }
}
