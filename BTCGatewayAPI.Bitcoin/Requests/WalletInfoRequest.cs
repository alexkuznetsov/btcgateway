using System;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    internal class WalletInfoRequest : CommandRequest
    {
        public WalletInfoRequest() : base(Guid.NewGuid().ToString(), Names.getwalletinfo)
        {
            Params = new object[] { };
        }
    }
}
