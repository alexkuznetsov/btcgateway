using System;
using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    [DataContract]
    public class CreateRawTransaction : CommandRequest
    {
        public CreateRawTransaction(Models.TXInfo[] inputs, System.Collections.Generic.Dictionary<string, decimal> reciviers)
            : base(Guid.NewGuid().ToString(), Names.createrawtransaction)
        {
            Params = new object[] { inputs, reciviers };
        }
    }
}
