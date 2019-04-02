using System;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    internal class DumpPrivKeyRequest : CommandRequest
    {
        public DumpPrivKeyRequest(string address) : base(Guid.NewGuid().ToString(), Names.dumpprivkey)
        {
            Params = new object[] { address };
        }
    }

    public class DumpPrivKeyResponse : CommandResponse<string>
    {

    }
}
