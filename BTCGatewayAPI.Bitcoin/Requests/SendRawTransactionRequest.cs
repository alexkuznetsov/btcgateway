using System;
using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    [DataContract]
    public class SendRawTransactionRequest : CommandRequest
    {
        public SendRawTransactionRequest(string txHash) : base(Guid.NewGuid().ToString(), Names.sendrawtransaction)
        {
            Params = new object[] { txHash };
        }
    }
}
