using Newtonsoft.Json;
using System;

namespace BTCGatewayAPI.Bitcoin.Models
{
    public class WalletInfoResult
    {
        /// <summary>
        /// (string) the wallet name
        /// </summary>
        [JsonProperty("walletname")]
        public string WalletName { get; set; }

        /// <summary>
        ///  (numeric) the wallet version
        /// </summary>
        [JsonProperty("walletversion")]
        public int WalletVersion { get; set; }

        /// <summary>
        /// (numeric) the total confirmed balance of the wallet in BTC
        /// </summary>
        [JsonProperty("balance")]
        public decimal Balance { get; set; }

        /// <summary>
        /// (numeric) the total unconfirmed balance of the wallet in BTC
        /// </summary>
        [JsonProperty("unconfirmed_balance")]
        public decimal UnconfirmedBalance { get; set; }

        /// <summary>
        /// (numeric) the total immature balance of the wallet in BTC
        /// </summary>
        [JsonProperty("immature_balance")]
        public decimal ImmatureBalance { get; set; }

        /// <summary>
        /// (numeric) the total number of transactions in the wallet
        /// </summary>
        [JsonProperty("txcount")]
        public int TxCount { get; set; }

        /// <summary>
        /// (numeric) the timestamp (seconds since Unix epoch) of the oldest pre-generated key in the key pool
        /// </summary>
        [JsonProperty("keypoololdest")]
        public int KeyPoolOldest { get; set; }

        /// <summary>
        /// (numeric) how many new keys are pre-generated (only counts external keys)
        /// </summary>
        [JsonProperty("keypoolsize")]
        public int KeyPoolSize { get; set; }

        /// <summary>
        /// how many new keys are pre-generated for internal use (used for change outputs, only appears if the wallet is using this feature, otherwise external keys are used)
        /// </summary>
        [JsonProperty("keypoolsize_hd_internal")]
        public int KeyPoolSizeHDInternal { get; set; }

        /// <summary>
        /// the timestamp in seconds since epoch (midnight Jan 1 1970 GMT) that the wallet is unlocked for transfers, or 0 if the wallet is locked
        /// </summary>
        [JsonProperty("unlocked_until")]
        public int UnlockedUntil { get; set; }

        /// <summary>
        /// the transaction fee configuration, set in BTC/kB
        /// </summary>
        [JsonProperty("paytxfee")]
        public decimal PayTFFee { get; set; }

        /// <summary>
        /// (string, optional) the Hash160 of the HD seed (only present when HD is enabled)
        /// </summary>
        [JsonProperty("hdseedid")]
        public string HDSeedId { get; set; }

        /// <summary>
        /// (string, optional) alias for hdseedid retained for backwards-compatibility
        /// </summary>
        [JsonProperty("hdmasterkeyid")]
        [Obsolete("Will be removed in V0.18.")]
        public string HDMasterKeyId { get; set; }

        /// <summary>
        /// (boolean) false if privatekeys are disabled for this wallet (enforced watch-only wallet)
        /// </summary>
        [JsonProperty("private_keys_enabled")]
        public bool PrivateKeysEnabled { get; set; }
    }
}
