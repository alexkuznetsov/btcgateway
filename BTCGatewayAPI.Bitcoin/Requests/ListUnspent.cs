﻿using System;
using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    [DataContract]
    public class ListUnspent : CommandRequest
    {
        public ListUnspent(int from = 0, int to = 9999999, string[] addresses = null)
            : base(Guid.NewGuid().ToString(), Names.listunspent)
        {
            Params = new object[] { from, to, addresses ?? new string[] { } };
        }
    }
}
