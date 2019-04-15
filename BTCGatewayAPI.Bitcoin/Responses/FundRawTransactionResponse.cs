using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Responses
{
    [DataContract]
    public class FundRawTransactionResponse : CommandResponse<Models.FundRawTransactionResult>
    {

    }
}
