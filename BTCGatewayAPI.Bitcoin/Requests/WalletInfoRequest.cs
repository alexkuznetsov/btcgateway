using System;
using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    [DataContract]
    public class WalletInfoRequest : CommandRequest
    {
        public WalletInfoRequest() : base(Guid.NewGuid().ToString(), Names.getwalletinfo)
        {
            Params = new object[] { };
        }
    }
}
