﻿using System;
using System.Runtime.CompilerServices;

namespace BTCGatewayAPI.Infrastructure
{
    public class GlobalConf
    {
        public class CSSettings
        {
            public string ProviderName { get; set; }
            public string ConnectionString { get; set; }
        }

        private readonly Func<string, string> _accessor;
        private readonly Func<string, CSSettings> _connStrAccessor;

        private CSSettings _ConnectionString;

        private int _ConfTargetForEstimateSmartFee = -1;
        private int _MinimalConfirmations = -1;
        private int _TXUpdateTimerInterval = -1;
        private int _LogSQL = -1;
        private int _LastTXMinimalConfirmations = -1;
        private int _WalletUnlockTime = -1;
        private int _UseFundRawTransaction = -1;
        private int _RetryActionCnt = -1;
        private int _IsTestNet = -1;
        private int _DefaultWalletUnlockTime = -1;

        public GlobalConf(Func<string, string> settingAccessor, Func<string, CSSettings> connStrAccessor)
        {
            _accessor = settingAccessor;
            _connStrAccessor = connStrAccessor;
        }

        public CSSettings ConnectionString
        {
            get
            {
                if (_ConnectionString == null)
                {
                    _ConnectionString = _connStrAccessor(DefaultSQLCS);
                }

                return _ConnectionString;
            }
        }

        public string DefaultSQLCS => "DefaultSQL";

        public int DefaultWalletUnlockTime => GetValueAndSetIfNotSet(ref _DefaultWalletUnlockTime, 10);

        public int ConfTargetForEstimateSmartFee => GetValueAndSetIfNotSet(ref _ConfTargetForEstimateSmartFee, 6);

        public int MinimalConfirmations => GetValueAndSetIfNotSet(ref _MinimalConfirmations, 6);

        public int LastTXMinimalConfirmations => GetValueAndSetIfNotSet(ref _LastTXMinimalConfirmations, 3);

        public int TXUpdateTimerInterval => GetValueAndSetIfNotSet(ref _TXUpdateTimerInterval, 60000);

        public bool LogSQL => GetValueAndSetIfNotSet(ref _LogSQL, true);

        public int WalletUnlockTime => GetValueAndSetIfNotSet(ref _WalletUnlockTime, 60);

        public bool UseFundRawTransaction => GetValueAndSetIfNotSet(ref _UseFundRawTransaction, true);

        public int RetryActionCnt => GetValueAndSetIfNotSet(ref _RetryActionCnt, 3);

        public bool IsTestNet => GetValueAndSetIfNotSet(ref _IsTestNet, true);

        private bool GetValueAndSetIfNotSet(ref int target, bool defaultVal, [CallerMemberName]string propName = "")
        {
            if (target == -1)
            {
                if (int.TryParse(_accessor(propName), out var temp))
                    target = temp;
                else
                    target = defaultVal ? 1 : 0;
            }

            return target == 1;
        }

        private int GetValueAndSetIfNotSet(ref int target, int defaultVal, [CallerMemberName]string propName = "")
        {
            if (target == -1)
            {
                if (int.TryParse(_accessor(propName), out var temp))
                    target = temp;
                else
                    target = defaultVal;
            }

            return target;
        }
    }
}
