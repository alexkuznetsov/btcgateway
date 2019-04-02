using BTCGatewayAPI.Bitcoin.Models;
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
        public string Transaction { get; set; }
        public string[] Keys { get; }
        public string SigHash { get; }
        public List<TxOutput> Outputs { get; set; } = new List<TxOutput>();

        public SignTransactionRequest(string txHash, string[] keys, string sigHash = Models.SigHash.ALL) : base(Guid.NewGuid().ToString(), Names.signtransaction)
        {
            Transaction = txHash;
            Keys = keys;
            SigHash = sigHash;
        }

        public void AddOutput(string txId, int vout, string scriptPubKey, string redeemScript)
        {
            Outputs.Add(new TxOutput { Txid = txId, Vout = vout, ScriptPubKey = scriptPubKey, RedeemScript = redeemScript });
        }

        public void AddOutput(TxOutput txOutput)
        {
            Outputs.Add(txOutput);
        }
    }

    public class SignTransactionResponse : CommandResponse<string>
    {

    }
}
