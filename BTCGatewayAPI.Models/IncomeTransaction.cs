using System;

namespace BTCGatewayAPI.Models
{
    public class IncomeTransaction : Entity
    {
        public DateTime Date { get; set; }

        public int WalletId { get; set; }

        public string TxHash { get; set; }

        public string Sender { get; set; }

        public decimal Amount { get; set; }

        public int ConfirmationCount { get; set; }
    }
}