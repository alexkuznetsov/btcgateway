using System;
using System.Net.Http;

namespace BTCGatewayAPI.Bitcoin
{
    public class BitcoinClientFactory
    {
        private readonly BitcoinClientOptions _conf;
        private readonly DelegatingHandler _sharedHadler;

        public BitcoinClientFactory(DelegatingHandler sharedHadler, BitcoinClientOptions conf)
        {
            _conf = conf;
            _sharedHadler = sharedHadler;
        }

        public BitcoinClient Create(Uri uri, string username, string password)
            => new BitcoinClient(_sharedHadler, _conf, uri, username, password);
    }
}