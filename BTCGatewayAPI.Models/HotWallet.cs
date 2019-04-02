using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTCGatewayAPI.Models
{
    [Obsolete]
    public class HowWalletId
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }
    }

    [Table("hot_wallets")]
    public class HotWallet : Entity
    {
        private static readonly decimal CheckDelta = new decimal(0.00000000001);

        [Column("address")]
        public string Address { get; set; }

        [Column("rpc_address")]
        public Uri RPCAddress { get; set; }

        [Column("rpc_username")]
        public string RPCUsername { get; set; }

        [Column("rpc_password")]
        public string RPCPassword { get; set; }

        [Column("amount")]
        public decimal Amount { get; set; }

        [Column("passphrase")]
        public string Passphrase { get; set; }

        public void Withdraw(decimal amount)
        {
            if (Amount - amount > CheckDelta)
            {
                Amount -= amount;
            }
            else
            {
                throw new InvalidOperationException("Try to withdraw more than excisting balance");
            }
        }
    }
}