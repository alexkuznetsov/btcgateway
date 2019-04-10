﻿using System;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    internal class DecodeRawTransactionRequest : CommandRequest
    {
        public DecodeRawTransactionRequest(string txhash, bool iswitness) : base(Guid.NewGuid().ToString(), Names.decoderawtransaction)
        {
            Params = new object[] { txhash, iswitness };
        }
    }
}
