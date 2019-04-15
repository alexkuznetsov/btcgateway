using System;
using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Models
{
    public class WalletInfoResult
    {
        /// <summary>
        /// (string) the wallet name
        /// </summary>
        [DataMember(Name = "walletname")]
        public string WalletName { get; set; }

        /// <summary>
        ///  (numeric) the wallet version
        /// </summary>
        [DataMember(Name = "walletversion")]
        public int WalletVersion { get; set; }

        /// <summary>
        /// (numeric) the total confirmed balance of the wallet in BTC
        /// </summary>
        [DataMember(Name = "balance")]
        public decimal Balance { get; set; }

        /// <summary>
        /// (numeric) the total unconfirmed balance of the wallet in BTC
        /// </summary>
        [DataMember(Name = "unconfirmed_balance")]
        public decimal UnconfirmedBalance { get; set; }

        /// <summary>
        /// (numeric) the total immature balance of the wallet in BTC
        /// </summary>
        [DataMember(Name = "immature_balance")]
        public decimal ImmatureBalance { get; set; }

        /// <summary>
        /// (numeric) the total number of transactions in the wallet
        /// </summary>
        [DataMember(Name = "txcount")]
        public int TxCount { get; set; }

        /// <summary>
        /// (numeric) the timestamp (seconds since Unix epoch) of the oldest pre-generated key in the key pool
        /// </summary>
        [DataMember(Name = "keypoololdest")]
        public int KeyPoolOldest { get; set; }

        /// <summary>
        /// (numeric) how many new keys are pre-generated (only counts external keys)
        /// </summary>
        [DataMember(Name = "keypoolsize")]
        public int KeyPoolSize { get; set; }

        /// <summary>
        /// how many new keys are pre-generated for internal use (used for change outputs, only appears if the wallet is using this feature, otherwise external keys are used)
        /// </summary>
        [DataMember(Name = "keypoolsize_hd_internal")]
        public int KeyPoolSizeHDInternal { get; set; }

        /// <summary>
        /// the timestamp in seconds since epoch (midnight Jan 1 1970 GMT) that the wallet is unlocked for transfers, or 0 if the wallet is locked
        /// </summary>
        [DataMember(Name = "unlocked_until")]
        public int UnlockedUntil { get; set; }

        /// <summary>
        /// the transaction fee configuration, set in BTC/kB
        /// </summary>
        [DataMember(Name = "paytxfee")]
        public decimal PayTFFee { get; set; }

        /// <summary>
        /// (string, optional) the Hash160 of the HD seed (only present when HD is enabled)
        /// </summary>
        [DataMember(Name = "hdseedid")]
        public string HDSeedId { get; set; }

        /// <summary>
        /// (string, optional) alias for hdseedid retained for backwards-compatibility
        /// </summary>
        [DataMember(Name = "hdmasterkeyid")]
        [Obsolete("Will be removed in V0.18.")]
        public string HDMasterKeyId { get; set; }

        /// <summary>
        /// (boolean) false if privatekeys are disabled for this wallet (enforced watch-only wallet)
        /// </summary>
        [DataMember(Name = "private_keys_enabled")]
        public bool PrivateKeysEnabled { get; set; }
    }
}
