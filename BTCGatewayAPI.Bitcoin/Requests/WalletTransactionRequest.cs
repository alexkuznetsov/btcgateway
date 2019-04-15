using System;
using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    [DataContract]
    public class WalletTransactionRequest : CommandRequest
    {
        public WalletTransactionRequest(string txid, bool includeWatchOnly)
            : base(Guid.NewGuid().ToString(), Names.gettransaction)
        {
            Params = new object[] { txid, includeWatchOnly };
        }
    }
}
