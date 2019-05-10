using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Responses
{
    [DataContract]
    public class SendToAddressResponse : CommandResponse<string>
    {

    }
}
