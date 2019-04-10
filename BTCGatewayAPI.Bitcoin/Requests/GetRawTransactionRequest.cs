using System;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    internal class GetRawTransactionRequest : CommandRequest
    {
        public GetRawTransactionRequest(string txid, bool verbose) : base(Guid.NewGuid().ToString(), Names.getrawtransaction)
        {
            Params = new object[] { txid, verbose };
        }
    }
}
