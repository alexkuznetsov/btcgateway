using System;
using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    [DataContract]
    public class RemovePrunedFundsRequest : CommandRequest
    {
        public RemovePrunedFundsRequest(string txHash) : base(Guid.NewGuid().ToString(), Names.removeprunedfunds)
        {
            Params = new object[] { txHash };
        }
    }
}
