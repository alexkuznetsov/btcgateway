using BTCGatewayAPI.Bitcoin.Models;
using System;
using System.Collections.Generic;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    internal class ListUnspent : CommandRequest
    {
        public ListUnspent(int from = 0, int to = 9999999, string[] addresses = null)
            : base(Guid.NewGuid().ToString(), Names.listunspent)
        {
            Params = new object[] { from, to, addresses ?? new string[] { } };
        }
    }

    public class ListUnspentResponse : CommandResponse<List<Unspent>>
    {
    }
}
