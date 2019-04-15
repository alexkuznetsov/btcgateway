using System;
using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    [DataContract]
    public class DumpPrivKeyRequest : CommandRequest
    {
        public DumpPrivKeyRequest(string address) : base(Guid.NewGuid().ToString(), Names.dumpprivkey)
        {
            Params = new object[] { address };
        }
    }
}
