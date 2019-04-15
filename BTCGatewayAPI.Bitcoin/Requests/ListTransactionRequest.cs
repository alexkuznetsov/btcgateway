using System;
using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    [DataContract]
    public class ListTransactionRequest : CommandRequest
    {
        public ListTransactionRequest(string dummy, int count, int skip, bool includeWatchOnly)
            : base(Guid.NewGuid().ToString(), Names.listtransactions)
        {
            Params = new object[] { /*dummy, count, skip, includeWatchOnly*/ };
        }
    }
}
