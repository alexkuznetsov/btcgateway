using System;

namespace BTCGatewayAPI.Models
{
    public class OutcomeTransaction : Entity
    {
        public int WalletId { get; set; }

        public string TxHash { get; set; }

        public string Recipient { get; set; }

        public decimal Amount { get; set; }
    }
}