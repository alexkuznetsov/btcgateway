using Newtonsoft.Json;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    public abstract class CommandRequest
    {
        [JsonProperty("jsonrpc", Order = 0)]
        public string Jsonrpc => "1.0";
        [JsonProperty("id", Order = 1)]
        public string Id { get; }
        [JsonProperty("method", Order = 2)]
        public string Method { get; }
        [JsonProperty("params", Order = 3)]
        public object[] Params { get; protected set; }

        protected CommandRequest(string id, string method)
        {
            Id = id;
            Method = method;
        }
    }
}
