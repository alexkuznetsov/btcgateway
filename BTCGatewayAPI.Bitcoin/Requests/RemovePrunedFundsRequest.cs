using System;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    internal class RemovePrunedFundsRequest : CommandRequest
    {
        public RemovePrunedFundsRequest(string txHash) : base(Guid.NewGuid().ToString(), Names.removeprunedfunds)
        {
            Params = new object[] { txHash };
        }
    }

    public class RemovePrunedFundsResponse : CommandResponse<string>
    {

    }
}
