using BTCGatewayAPI.Bitcoin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
