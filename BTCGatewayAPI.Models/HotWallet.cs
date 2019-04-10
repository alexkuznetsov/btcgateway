using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTCGatewayAPI.Models
{
    [Table("hot_wallets")]
    public class HotWallet : Entity
    {
        private static readonly decimal CheckDelta = new decimal(0.00000000001);

        [Column("address")]
        public string Address { get; set; }
        
        [Column("amount")]
        public decimal Amount { get; set; }

        [Column("tx_count")]
        public int TxCount { get; set; }

        #region RPC and sec

        [Column("rpc_address")]
        public string RPCAddress { get; set; }

        [Column("rpc_username")]
        public string RPCUsername { get; set; }

        [Column("rpc_password")]
        public string RPCPassword { get; set; }

        [Column("passphrase")]
        public string Passphrase { get; set; }

        #endregion


        public void Withdraw(decimal amount)
        {
            if (Amount - amount > CheckDelta)
            {
                Amount -= amount;
            }
            else
            {
                throw new InvalidOperationException(Messages.ErrorTryToWithdrawMoreThanExists);
            }
        }
    }
}