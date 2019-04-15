using System.Runtime.Serialization;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    [DataContract]
    public abstract class CommandRequest
    {
        [DataMember(Name = "jsonrpc", Order = 0)]
        public string Jsonrpc => "1.0";

        [DataMember(Name = "id", Order = 1)]
        public string Id { get; }

        [DataMember(Name = "method", Order = 2)]
        public string Method { get; }

        [DataMember(Name = "params", Order = 3)]
        public object[] Params { get; protected set; }

        protected CommandRequest(string id, string method)
        {
            Id = id;
            Method = method;
        }
    }
}
