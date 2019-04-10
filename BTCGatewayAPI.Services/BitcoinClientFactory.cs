﻿using System;
using System.Net.Http;

namespace BTCGatewayAPI.Services
{
    public class BitcoinClientFactory
    {
        private readonly Infrastructure.GlobalConf _conf;
        private readonly DelegatingHandler _sharedHadler;

        public BitcoinClientFactory(Infrastructure.GlobalConf conf, DelegatingHandler sharedHadler)
        {
            _conf = conf;
            _sharedHadler = sharedHadler;
        }

        public BitcoinClient Create(Uri uri, string username, string password)
            => new BitcoinClient(_sharedHadler, _conf, uri, username, password);
    }
}