using System;
using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    [DataContract]
    public class FundRawTransactionRequest : CommandRequest
    {
        public FundRawTransactionRequest(string txHash, Models.FundRawTransactionOptions options)
            : base(Guid.NewGuid().ToString(), Names.fundrawtransaction)
        {
            Params = new object[] { txHash, options };
        }

        
    }
}
