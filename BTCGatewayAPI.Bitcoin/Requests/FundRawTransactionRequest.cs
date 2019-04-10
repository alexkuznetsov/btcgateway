using System;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    internal class FundRawTransactionRequest : CommandRequest
    {
        public FundRawTransactionRequest(string txHash, Models.FundRawTransactionOptions options)
            : base(Guid.NewGuid().ToString(), Names.fundrawtransaction)
        {
            Params = new object[] { txHash, options };
        }

        
    }
}
