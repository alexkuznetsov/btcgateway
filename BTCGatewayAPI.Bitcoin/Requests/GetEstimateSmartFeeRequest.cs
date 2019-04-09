using System;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    internal class GetEstimateSmartFeeRequest : CommandRequest
    {
        public GetEstimateSmartFeeRequest(int confTarget) :
            base(Guid.NewGuid().ToString(), Names.estimatesmartfee)
        {
            Params = new object[] { confTarget };
        }
    }
}
