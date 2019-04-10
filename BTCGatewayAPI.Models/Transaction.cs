using System.ComponentModel.DataAnnotations.Schema;

namespace BTCGatewayAPI.Models
{
    public abstract class Transaction : Entity
    {
        [Column("address")]
        public string Address { get; set; }

        [Column("tx_id")]
        public string Txid { get; set; }

        [Column("tx_hash")]
        public string TxHash { get; set; }

        [Column("amount")]
        public decimal Amount { get; set; }

        [Column("confirmation")]
        public int Confirmations { get; set; }

        [Column("wallet_id")]
        public int WalletId { get; set; }
    }
}