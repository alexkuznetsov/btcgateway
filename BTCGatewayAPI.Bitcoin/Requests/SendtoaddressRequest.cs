using System;
using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    [DataContract]
    public class SendToAddressRequest : CommandRequest
    {
        public SendToAddressRequest(string address, decimal amount, string commentFrom = null, string commentTo = null,
                                    bool? subtractfeefromamount = null, bool? replaceable = null,
                                    int? confTarget = null, string estimateMode = null) : base(Guid.NewGuid().ToString(), Names.sendtoaddress)
        {
            Params = new object[] { address, amount, commentFrom, commentTo };
        }
    }
}
