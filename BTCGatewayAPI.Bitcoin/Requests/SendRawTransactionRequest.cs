using System;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    internal class SendRawTransactionRequest : CommandRequest
    {
        public SendRawTransactionRequest(string txHash) : base(Guid.NewGuid().ToString(), Names.sendrawtransaction)
        {
            Params = new object[] { txHash };
        }
    }
}
