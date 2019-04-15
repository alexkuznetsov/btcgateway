using BTCGatewayAPI.Bitcoin.Models;
using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Responses
{
    [DataContract]
    public class GetEstimateSmartFeeResponce : CommandResponse<EstimateSmartFee>
    {

    }
}
