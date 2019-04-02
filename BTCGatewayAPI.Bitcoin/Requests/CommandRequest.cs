using Newtonsoft.Json;

namespace BTCGatewayAPI.Bitcoin.Requests
{
    public static class Names
    {
        public const string estimatesmartfee = nameof(estimatesmartfee);
        public const string createrawtransaction = nameof(createrawtransaction);
        public const string listunspent = nameof(listunspent);
        public const string signtransaction = nameof(signtransaction);
        public const string sendrawtransaction = nameof(sendrawtransaction);
        public const string removeprunedfunds = nameof(removeprunedfunds);
        public const string walletpassphrase = nameof(walletpassphrase);
        public const string dumpprivkey = nameof(dumpprivkey);
    }

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
