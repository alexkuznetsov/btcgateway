using System;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    internal class WalletPassphraseRequest : CommandRequest
    {
        public WalletPassphraseRequest(string passphrase, int seconds) : base(Guid.NewGuid().ToString(), Names.walletpassphrase)
        {
            Params = new object[] { passphrase, seconds };
        }
    }

    public class WalletPassphraseResponse : CommandResponse<string>
    {

    }
}
