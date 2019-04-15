using BTCGatewayAPI.Bitcoin.Models;
using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Responses
{
    [DataContract]
    public class SignTransactionResponse : CommandResponse<SignTransactionResult>
    {

    }
}
