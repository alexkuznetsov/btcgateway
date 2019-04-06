using System.ComponentModel.DataAnnotations.Schema;

namespace BTCGatewayAPI.Models
{
    public abstract class Transaction : Entity
    {
        [Column("wallet_id")]
        public int WalletId { get; set; }

        [Column("tx_hash")]
        public string TxHash { get; set; }

        [Column("amount")]
        public decimal Amount { get; set; }

        [Column("confirmation")]
        public int Confirmations { get; set; }
    }
}