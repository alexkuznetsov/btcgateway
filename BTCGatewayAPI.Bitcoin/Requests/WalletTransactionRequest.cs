using System;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    internal class WalletTransactionRequest : CommandRequest
    {
        public WalletTransactionRequest(string txid, bool includeWatchOnly)
            : base(Guid.NewGuid().ToString(), Names.gettransaction)
        {
            Params = new object[] { txid, includeWatchOnly };
        }
    }
}
