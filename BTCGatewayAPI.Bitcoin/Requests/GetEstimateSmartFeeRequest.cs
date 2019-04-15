using System;
using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    [DataContract]
    public class GetEstimateSmartFeeRequest : CommandRequest
    {
        public GetEstimateSmartFeeRequest(int confTarget) :
            base(Guid.NewGuid().ToString(), Names.estimatesmartfee)
        {
            Params = new object[] { confTarget };
        }
    }
}
