using System;
using System.Runtime.Serialization;

namespace BTCGatewayAPI.Models.DTO
{
    [DataContract]
    public class HotWalletDTO
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [DataMember(Name = "address")]
        public string Address { get; set; }

        [DataMember(Name = "amount")]
        public decimal Amount { get; set; }
    }
}
