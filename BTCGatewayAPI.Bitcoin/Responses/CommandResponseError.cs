using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Responses
{
    public class CommandResponseError
    {
        [DataMember(Name = "code", Order = 0)]
        public int Code { get; set; }

        [DataMember(Name = "message", Order = 1)]
        public string Message { get; set; }
    }
}
