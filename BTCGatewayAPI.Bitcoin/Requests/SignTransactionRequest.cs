using BTCGatewayAPI.Bitcoin.Models;
using System;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    /// <summary>
    /// <para>
    /// https://bitcoin.org/en/developer-reference#signrawtransaction
    /// </para>
    /// </summary>
    internal class SignTransactionRequest : CommandRequest
    {
        public SignTransactionRequest(string txHash, string[] keys, TxOutput[] outputs, string sigHash = Models.SigHash.ALL) :
            base(Guid.NewGuid().ToString(), Names.signrawtransactionwithkey)
        {
            Params = new object[] {
                txHash,keys, outputs, sigHash
            };

        }
    }
}
