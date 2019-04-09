using System;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    internal class CreateRawTransaction : CommandRequest
    {
        public CreateRawTransaction(Models.TXInfo[] inputs, System.Collections.Generic.Dictionary<string, decimal> reciviers)
            : base(Guid.NewGuid().ToString(), Names.createrawtransaction)
        {
            Params = new object[] { inputs, reciviers };
        }
    }
}
