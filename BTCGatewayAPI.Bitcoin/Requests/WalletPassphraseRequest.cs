using System;
using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    [DataContract]
    public class WalletPassphraseRequest : CommandRequest
    {
        public WalletPassphraseRequest(string passphrase, int seconds) : base(Guid.NewGuid().ToString(), Names.walletpassphrase)
        {
            Params = new object[] { passphrase, seconds };
        }
    }
}
