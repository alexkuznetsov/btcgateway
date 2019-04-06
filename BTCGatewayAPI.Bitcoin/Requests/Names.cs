﻿namespace BTCGatewayAPI.Bitcoin.Requests
{
    public static class Names
    {
        public const string estimatesmartfee = nameof(estimatesmartfee);
        public const string createrawtransaction = nameof(createrawtransaction);
        public const string listunspent = nameof(listunspent);
        public const string signrawtransactionwithkey = nameof(signrawtransactionwithkey);
        public const string sendrawtransaction = nameof(sendrawtransaction);
        public const string removeprunedfunds = nameof(removeprunedfunds);
        public const string walletpassphrase = nameof(walletpassphrase);
        public const string dumpprivkey = nameof(dumpprivkey);
        public const string listtransactions = nameof(listtransactions);
    }
}