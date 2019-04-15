using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Responses
{
    [DataContract]
    public class WalletPassphraseResponse : CommandResponse<string>
    {

    }
}
