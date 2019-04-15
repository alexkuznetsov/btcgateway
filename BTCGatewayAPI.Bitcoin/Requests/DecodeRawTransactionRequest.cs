using System;
using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    [DataContract]
    public class DecodeRawTransactionRequest : CommandRequest
    {
        public DecodeRawTransactionRequest(string txhash, bool iswitness) : base(Guid.NewGuid().ToString(), Names.decoderawtransaction)
        {
            Params = new object[] { txhash/*, iswitness*/ };
        }
    }
}
