using System;
using System.Net.Http;

namespace BTCGatewayAPI.Services
{
    public class BitcoinClientFactory
    {
        private readonly Infrastructure.GlobalConf conf;
        private readonly DelegatingHandler sharedHadler;

        public BitcoinClientFactory(Infrastructure.GlobalConf conf, DelegatingHandler sharedHadler)
        {
            this.conf = conf;
            this.sharedHadler = sharedHadler;
        }

        public BitcoinClient Create(Uri uri, string username, string password)
            => new BitcoinClient(sharedHadler, conf, uri, username, password);
    }
}