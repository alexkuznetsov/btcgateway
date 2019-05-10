using BTCGatewayAPI.Bitcoin.Models;
using System;
using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    /// <summary>
    /// <para>
    /// https://bitcoin.org/en/developer-reference#signrawtransaction
    /// </para>
    /// </summary>
    [DataContract]
    public class SignTransactionRequest : CommandRequest
    {
        public const string SigHash_ALL = "ALL";
        public const string SigHash_NONE = "NONE";
        public const string SigHash_SINGLE = "SINGLE";
        public const string SigHash_ALL_ANYONECANPAY = "ALL|ANYONECANPAY";
        public const string SigHash_NONE_ANYONECANPAY = "NONE|ANYONECANPAY";
        public const string SigHash_SINGLE_ANYONECANPAY = "SINGLE|ANYONECANPAY";

        public SignTransactionRequest(string txHash, string[] keys, TxOutput[] outputs, string sigHash = SigHash_ALL) :
            base(Guid.NewGuid().ToString(), Names.signrawtransactionwithkey)
        {
            Params = new object[] {
                txHash, keys, outputs, sigHash
            };

        }
    }
}
