﻿using System;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    internal class CreateRawTransaction : CommandRequest
    {
        public CreateRawTransaction(Models.TXInfo[] inputs, Models.FundRecivier[] reciviers)
            : base(Guid.NewGuid().ToString(), Names.createrawtransaction)
        {
            Params = new object[] { inputs, reciviers };
        }
    }
    public class CreateRawTransactionResponse : CommandResponse<string>
    {

    }
}