using BTCGatewayAPI.Bitcoin;
using System;
using System.Net.Http;

namespace BTCGatewayAPI.Services
{
    public class BitcoinClientFactory
    {
        private readonly int confTargetForEstimateSmartFee;
        private readonly DelegatingHandler sharedHadler;

        public BitcoinClientFactory(int confTargetForEstimateSmartFee, DelegatingHandler sharedHadler)
        {
            this.confTargetForEstimateSmartFee = confTargetForEstimateSmartFee;
            this.sharedHadler = sharedHadler;
        }

        public BitcoinClient Create(Uri uri, string username, string password)
        {
            var server = new RPCServer(sharedHadler, uri, username, password);
            return new BitcoinClient(server, confTargetForEstimateSmartFee);
        }
    }
}