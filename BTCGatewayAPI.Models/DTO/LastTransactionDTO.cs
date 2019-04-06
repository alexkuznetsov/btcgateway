using Newtonsoft.Json;
using System;

namespace BTCGatewayAPI.Models.DTO
{
    public class LastTransactionDTO
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("confirmation")]
        public int Confirmation { get; set; }
    }
}
