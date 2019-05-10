namespace BTCGatewayAPI.Common
{
    public class GlobalConf
    {
        public class CSSettings
        {
            public string ProviderName { get; set; }

            public string ConnectionString { get; set; }
        }

        public CSSettings ConnectionString { get; set; }

        public int DefaultWalletUnlockTime { get; set; } = 10;

        public int ConfTargetForEstimateSmartFee { get; set; } = 6;

        public int MinimalConfirmations { get; set; } = 6;

        public int LastTXMinimalConfirmations { get; set; } = 3;

        public int TXUpdateTimerInterval { get; set; } = 60000;

        public int WalletUnlockTime { get; set; } = 60;

        public bool UseFundRawTransaction { get; set; } = true;

        public int RetryActionCnt { get; set; } = 3;

        public bool IsTestNet { get; set; } = true;
    }
}
