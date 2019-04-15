using System;
using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    [DataContract]
    public class GetRawTransactionRequest : CommandRequest
    {
        public GetRawTransactionRequest(string txid, bool verbose) : base(Guid.NewGuid().ToString(), Names.getrawtransaction)
        {
            Params = new object[] { txid, verbose };
        }
    }
}
