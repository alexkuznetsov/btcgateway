using BTCGatewayAPI.Bitcoin.Models;
using System;
using System.Collections.Generic;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    internal class ListTransactionRequest : CommandRequest
    {
        public ListTransactionRequest(string dummy, int count, int skip, bool includeWatchOnly)
            : base(Guid.NewGuid().ToString(), Names.listtransactions)
        {
            Params = new object[] { /*dummy, count, skip, includeWatchOnly*/ };
        }
    }

    public class ListTransactionResponse : CommandResponse<List<WalletTransaction>>
    {

    }
}
